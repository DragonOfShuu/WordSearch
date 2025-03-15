import { Injectable, signal } from '@angular/core';
import SignalRSocket from '../../shared/signal-r-socket/signal-r-socket.class';
import Difficulty from '../../shared/types/difficulty.type';
import { Board } from '../../shared/types/boards.types';
import { FindWordResults } from '../../shared/types/find-word-results.types';
import { Observable, ReplaySubject } from 'rxjs';
import { Vector2D } from '../../shared/types/vector.types';

@Injectable({
  providedIn: 'root',
})
export class SingleplayerService {
  socket: SignalRSocket;
  connectionObservaboo: ReplaySubject<void>;
  currentBoard = signal<Board | null>(null);

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
  }

  async newGame(difficulty: Difficulty) {
    const newBoard: Board = await this.socket.invoke('NewGame', difficulty);
    console.log(`Board received: ${JSON.stringify(newBoard)}`);
    this.currentBoard.update(() => newBoard);
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
  ): Promise<FindWordResults | null> {
    const results: null | FindWordResults = await this.socket.invoke(
      'findWord',
      start,
      direction,
      count,
    );
    console.log('FindWordResults as: ', results);
    if (!results) return null;

    this.currentBoard.update(() => results.board);
    return results;
  }

  $verifyBoard(board: Board | undefined | null): Board {
    if (!board) throw new Error('Board has not been initialized');
    return board;
  }
}
