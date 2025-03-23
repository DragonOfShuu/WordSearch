import { Component, input } from '@angular/core';
import { WordDictionary } from '../../../../shared/types/word-dictionary.types';

@Component({
  selector: 'shuu-words-to-find',
  imports: [],
  templateUrl: './words-to-find.component.html',
  styleUrl: './words-to-find.component.sass'
})
export class WordsToFindComponent {
  wordsFound = input<null|WordDictionary>(null)
  wordsToFind = input<string[]>([])

  isWordFound(word: string) {
    const found = this.wordsFound()
    if (!found) return;
    return Object.keys(found).includes(word);
  }
}
