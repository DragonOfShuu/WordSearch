import { Component, input, output } from '@angular/core';
import {
  WordDictionary,
  WordType,
} from '../../../shared/types/word-dictionary.types';

@Component({
  selector: 'shuu-wordsearch',
  templateUrl: './wordsearch.component.html',
  styleUrl: './wordsearch.component.sass',
})
export class WordsearchComponent {
  searchableText = input.required<string[][]>();
  foundText = input.required<WordDictionary>();

  selectedText = output<WordType>();

  tileClicked(x: number, y: number) {}
}
