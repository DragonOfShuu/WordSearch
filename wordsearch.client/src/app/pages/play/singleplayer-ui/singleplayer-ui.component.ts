import {
  Component,
  computed,
  inject,
  input,
  OnInit,
  signal,
} from '@angular/core';
import { SingleplayerService } from '../../../core/singleplayer/singleplayer.service';
import { WordsearchComponent } from '../../../core/wordsearch-box/wordsearch/wordsearch.component';
import { WordType } from '../../../shared/types/word-dictionary.types';
import { DecimalPipe } from '@angular/common';
import { RoundPipe } from '../../../shared/round/round.pipe';
import { WordsToFindComponent } from "./words-to-find/words-to-find.component";
import { Board } from '../../../shared/types/boards.types';
import { toObservable } from '@angular/core/rxjs-interop';

@Component({
  selector: 'shuu-singleplayer-ui',
  providers: [SingleplayerService],
  templateUrl: './singleplayer-ui.component.html',
  styleUrl: './singleplayer-ui.component.sass',
  imports: [WordsearchComponent, DecimalPipe, RoundPipe, WordsToFindComponent],
})
export class SingleplayerUiComponent implements OnInit {
  singleplayerService = inject(SingleplayerService);
  leaveFunction = input<() => void>();
  board = this.singleplayerService.currentBoard;
  boardChanged$ = toObservable(this.board)
  currentBoard = signal<Board|null>(null);
  boardCharacters = computed(
    () =>
      this.currentBoard()?.boardCharacters ?? [],
  );
  foundWords = computed(() => this.currentBoard()?.found ?? {});
  timeRemaining = signal<number | null>(300);
  timeRemainingInterval: number | null = null;
  loadingBoard = signal(false);
  boardTransition = signal(false)

  constructor() {
    this.singleplayerService.registerBoardUpdate((updateInfo) => {
      console.log("Received from service: ", updateInfo)
      if (!updateInfo.brandNewBoard) {
        this.currentBoard.set(updateInfo.update);
        return;
      }

      this.currentBoard.set(updateInfo.update);
      setTimeout(() => {
        this.boardTransition.set(true);
        setTimeout(() => {
          this.boardTransition.set(false);
          this.currentBoard.set(updateInfo.brandNewBoard)
        }, 300)
      }, 700);
    })
  }

  ngOnInit(): void {
    // Cause ✨ JavaScript ✨
    const betterThis = this;
    betterThis.loadingBoard.set(true);
    this.singleplayerService.connectionObservaboo.subscribe({
      async complete() {
        await betterThis.singleplayerService.newGame({
          intensity: 'medium',
          level: 1,
          time: 300,
        });
        betterThis.loadingBoard.set(false);

        betterThis.timeRemainingInterval = window.setInterval(
          betterThis.recalcTimeRemaining.bind(betterThis),
          500,
          [300],
        );
      },
    });
  }

  findWord(word: WordType) {
    this.singleplayerService.findWord(
      word.position,
      word.rotation,
      word.word.length,
    );
  }

  recalcTimeRemaining(totalSeconds: number) {
    const board = this.currentBoard();

    if (board === null) {
      this.timeRemaining.set(null);
      return;
    }

    /** In milliseconds */
    const timeLeft = Math.max(
      totalSeconds * 1000 - (Date.now() - board.started),
      0,
    );
    this.timeRemaining.set(timeLeft / 1000);
  }
}
