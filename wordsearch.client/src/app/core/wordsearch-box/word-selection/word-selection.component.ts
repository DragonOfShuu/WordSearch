import { Component, input, OnInit } from '@angular/core';
import { randomInt } from '../../../shared/random/random';

@Component({
  selector: 'shuu-word-selection',
  imports: [],
  templateUrl: './word-selection.component.html',
  styleUrl: './word-selection.component.sass',
})
export class WordSelectionComponent {
  r = input(this.randomColorValue());
  g = input(this.randomColorValue());
  b = input(this.randomColorValue());

  randomColorValue() {
    return randomInt(50, 240);
  }
}
