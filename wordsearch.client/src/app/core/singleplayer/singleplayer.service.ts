import { Injectable, OnInit, signal } from '@angular/core';
import SignalRSocket from '../../shared/signal-r-socket/signal-r-socket.class';
import Difficulty from '../../shared/types/difficulty.type';
import { Board } from '../../shared/types/boards.types';
import { ReplaySubject, Subject } from 'rxjs';
import { Vector2D } from '../../shared/types/vector.types';
import { BoardUpdateType } from '../../shared/types/board-update.type';

@Injectable({
  providedIn: 'root',
})
export class SingleplayerService {
  socket: SignalRSocket;
  connectionObservaboo!: ReplaySubject<void>;
  currentBoard = signal<Board | null>(null);
  $boardUpdateEvent = new Subject<{
    update: Board;
    brandNewBoard: Board | null;
  }>();

  constructor() {
    this.socket = new SignalRSocket('/hubs/singleplayer');
    this.connectionObservaboo = this.socket.startConnection();

    this.connectionObservaboo.subscribe({
      complete() {
        console.log('Singleplayer connection success!');
      },
      error(err) {
        console.log(`Connection failed because of ${err}`);
      },
    });

    this.socket.registerOnMethod('boardUpdate', (data) =>
      this.boardUpdate(data as [BoardUpdateType]),
    );
  }

  async newGame(difficulty: Difficulty) {
    const newBoard: Board = await this.socket.invoke('NewGame', difficulty);
    console.log(`New Game Received!: ${JSON.stringify(newBoard)}`);
    this.currentBoard.update(() => newBoard);
    this.$boardUpdateEvent.next({ update: newBoard, brandNewBoard: null });
    return newBoard;
  }

  getConnectionState(): signalR.HubConnectionState {
    return this.socket.getConnectionState();
  }

  getDifficulty() {
    return this.$verifyBoard(this.currentBoard()).difficulty;
  }

  getBoard() {
    return this.$verifyBoard(this.currentBoard());
  }

  getFindableWords() {
    return this.$verifyBoard(this.currentBoard()).findable;
  }

  getFoundWords() {
    return this.$verifyBoard(this.currentBoard()).found;
  }

  async findWord(
    start: Vector2D,
    direction: Vector2D,
    count: number,
  ): Promise<boolean | null> {
    const results: null | boolean = await this.socket.invoke(
      'findWord',
      start,
      direction,
      count,
    );
    console.log('FindWordResults as: ', results);
    if (!results) return null;

    return results;
  }

  boardUpdate(args: [BoardUpdateType]) {
    const updates = args[0];
    const newBoard = updates.newBoard ?? updates.board;

    console.log(`Current board: `, this.currentBoard());
    console.log(`Board received: `, newBoard);

    this.currentBoard.set(newBoard);
    this.$boardUpdateEvent.next({
      update: updates.board,
      brandNewBoard: updates.newBoard ?? null,
    });
    return newBoard;
  }

  registerBoardUpdate(
    func: (x: { update: Board; brandNewBoard: Board | null }) => void,
  ) {
    console.log('Register updates....');
    this.$boardUpdateEvent.subscribe(func);
  }

  $verifyBoard(board: Board | undefined | null): Board {
    if (!board) throw new Error('Board has not been initialized');
    return board;
  }
}
