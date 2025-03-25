import { Component, computed, effect, input, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { WordDictionary } from '../../../../shared/types/word-dictionary.types';

@Component({
  selector: 'shuu-words-to-find',
  imports: [],
  templateUrl: './words-to-find.component.html',
  styleUrl: './words-to-find.component.sass',
})
export class WordsToFindComponent {
  wordsFound = input<null | WordDictionary>(null);
  wordsToFind = input<string[]>([]);
  wordsInfo = signal<null | {
    wordsFound: WordDictionary;
    wordsToFind: string[];
  }>(null);
  onGoingTransition = signal<boolean>(false);

  toFindChanged$ = toObservable(this.wordsToFind);

  constructor() {
    this.toFindChanged$.subscribe((value) => {
      const wordInfo = this.wordsInfo();
      if (
        wordInfo &&
        wordInfo.wordsToFind.every((word) => this.wordsToFind().includes(word))
      ) {
        const found = this.wordsFound();
        this.wordsInfo.set(
          found === null ? null : { wordsToFind: value, wordsFound: found },
        );
        return;
      }
      this.onGoingTransition.set(true);
      setTimeout(() => {
        this.onGoingTransition.set(false);
        const found = this.wordsFound();
        this.wordsInfo.set(
          found === null ? null : { wordsToFind: value, wordsFound: found },
        );
      }, 200);
    });
  }

  isWordFound(word: string) {
    const found = this.wordsInfo()?.wordsFound;
    if (!found) return;
    return Object.keys(found).includes(word);
  }
}
