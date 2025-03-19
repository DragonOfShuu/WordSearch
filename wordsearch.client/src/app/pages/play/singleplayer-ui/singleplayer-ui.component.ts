import { Component, computed, inject, input, OnInit, signal } from '@angular/core';
import { SingleplayerService } from '../../../core/singleplayer/singleplayer.service';
import { WordsearchComponent } from '../../../core/wordsearch-box/wordsearch/wordsearch.component';
import { WordType } from '../../../shared/types/word-dictionary.types';
import { DecimalPipe } from '@angular/common';
import { RoundPipe } from "../../../shared/round/round.pipe";

@Component({
  selector: 'shuu-singleplayer-ui',
  providers: [SingleplayerService],
  templateUrl: './singleplayer-ui.component.html',
  styleUrl: './singleplayer-ui.component.sass',
  imports: [WordsearchComponent, DecimalPipe, RoundPipe],
})
export class SingleplayerUiComponent implements OnInit {
  singleplayerService = inject(SingleplayerService);
  leaveFunction = input<() => void>();
  currentBoard = this.singleplayerService.currentBoard;
  boardCharacters = computed(
    () =>
      this.currentBoard()?.boardCharacters ?? Array(5).fill(Array(5).fill('A')),
  );
  foundWords = computed(() => this.currentBoard()?.found ?? {});
  timeRemaining = signal<number|null>(300);
  timeRemainingInterval: number|null = null;

  ngOnInit(): void {
    // Cause ✨ JavaScript ✨
    const betterThis = this;
    this.singleplayerService.connectionObservaboo.subscribe({
      async complete() {
        const board = await betterThis.singleplayerService.newGame({
          intensity: 'medium',
          level: 2,
          time: 300,
        });

        betterThis.timeRemainingInterval = window.setInterval(betterThis.recalcTimeRemaining.bind(betterThis), 500, [300])
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
    const timeLeft = Math.max(totalSeconds*1000 - (Date.now() - board.started), 0);
    this.timeRemaining.set(
      timeLeft / 1000
    )
  }

  wordFound(word: string) {
    return Object.keys(this.foundWords()).includes(word);
  }
}
