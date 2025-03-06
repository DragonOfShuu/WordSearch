import { Component, input, OnInit } from '@angular/core';
import { randomInt } from '../../../shared/random/random';
import { Vector2D } from '../../../shared/types/vector.types';
import { WordType } from '../../../shared/types/word-dictionary.types';

@Component({
  selector: 'shuu-word-selection',
  imports: [],
  templateUrl: './word-selection.component.html',
  styleUrl: './word-selection.component.sass'
})
export class WordSelectionComponent {
  r = input(this.randomColorValue());
  g = input(this.randomColorValue());
  b = input(this.randomColorValue());

  position = input.required<Vector2D>()
  foundCoords = input.required<{[x: string]: WordType[]}>()
  selected = input.required<null|Vector2D>()
  hover = input.required<null|Vector2D>()

  randomColorValue() {
    return randomInt(50, 240);
  }
}
