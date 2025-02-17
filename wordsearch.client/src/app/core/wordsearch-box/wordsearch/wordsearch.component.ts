import { Component, input } from '@angular/core';

@Component({
  selector: 'shuu-wordsearch',
  standalone: false,

  templateUrl: './wordsearch.component.html',
  styleUrl: './wordsearch.component.sass',
})
export class WordsearchComponent {
  searchableText = input.required<string[][]>();
}
