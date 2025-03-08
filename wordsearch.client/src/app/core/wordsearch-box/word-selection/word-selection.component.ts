import { Component, computed, input, OnInit } from '@angular/core';
import { randomInt } from '../../../shared/random/random';
import { Vector2D } from '../../../shared/types/vector.types';
import { WordType } from '../../../shared/types/word-dictionary.types';
import { RgbPipePipe as RgbifyPipe } from '../../../shared/rgb-pipe/rgbify.pipe';

@Component({
  selector: 'shuu-word-selection',
  imports: [RgbifyPipe],
  templateUrl: './word-selection.component.html',
  styleUrl: './word-selection.component.sass',
})
export class WordSelectionComponent {
  r = input(this.randomColorValue());
  g = input(this.randomColorValue());
  b = input(this.randomColorValue());
  rgb = computed(() => {
    return { r: this.r(), g: this.g(), b: this.b() };
  });

  position = input.required<Vector2D>();
  rotation = input.required<Vector2D | null>();
  length = input.required<number | null>();

  randomColorValue() {
    return randomInt(50, 240);
  }
}
