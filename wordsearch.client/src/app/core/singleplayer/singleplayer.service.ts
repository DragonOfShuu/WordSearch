import { Injectable, signal } from '@angular/core';
import SignalRSocket from '../../shared/signal-r-socket/signal-r-socket.class';
import Difficulty from '../../shared/types/difficulty.type';
import { Board } from '../../shared/types/boards.types';
import { FindWordResults } from '../../shared/types/find-word-results.types';

@Injectable({
  providedIn: 'root',
})
export class SingleplayerService {
  socket: SignalRSocket
  currentBoard = signal<Board|null>(null);
  
  constructor() { 
    this.socket = new SignalRSocket("/hubs/singleplayer");
  }

  async newGame(difficulty: Difficulty) {
    const newBoard: Board = await this.socket.invoke('NewGame', difficulty)
    this.currentBoard.update(() => newBoard);
    return newBoard;
  }

  getDifficulty() {
    return this.$verifyBoard(this.currentBoard()).Difficulty;
  }

  getBoard() {
    return this.$verifyBoard(this.currentBoard());
  }

  getFindableWords() {
    return this.$verifyBoard(this.currentBoard()).Findable
  }

  getFoundWords() {
    return this.$verifyBoard(this.currentBoard()).Found
  }

  async findWord(start: [number, number], direction: [number, number], count: number): Promise<FindWordResults|null> {
    const results: null|FindWordResults = await this.socket.invoke('FindWord', start, direction, count);
    if (!results) return null;

    this.currentBoard.update(() => results.Board);
    return results;
  }

  $verifyBoard(board: Board|undefined|null): Board {
    if (!board)
      throw new Error("Board has not been initialized");
    return board;
  }
}
