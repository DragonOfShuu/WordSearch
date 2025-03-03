import { Component, input } from '@angular/core';

@Component({
  selector: 'shuu-wordsearch',
  templateUrl: './wordsearch.component.html',
  styleUrl: './wordsearch.component.sass',
})
export class WordsearchComponent {
  searchableText = input.required<string[][]>();
}
