import { Component, computed, inject, input, OnInit } from '@angular/core';
import { SingleplayerService } from '../../../core/singleplayer/singleplayer.service';
import { WordsearchComponent } from '../../../core/wordsearch-box/wordsearch/wordsearch.component';
import { WordType } from '../../../shared/types/word-dictionary.types';

@Component({
  selector: 'shuu-singleplayer-ui',
  providers: [SingleplayerService],
  templateUrl: './singleplayer-ui.component.html',
  styleUrl: './singleplayer-ui.component.sass',
  imports: [WordsearchComponent],
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

  ngOnInit(): void {
    // Cause ✨ JavaScript ✨
    const betterThis = this;
    this.singleplayerService.connectionObservaboo.subscribe({
      complete() {
        betterThis.singleplayerService.newGame({
          intensity: 'medium',
          level: 3,
          time: 300,
        });
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
}
