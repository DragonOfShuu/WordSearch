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

  rotationToTransform(rot: Vector2D | null): string {
    if (rot === null) return `rotate(0)`;

    return `rotate(${this.fromRotation(rot)}deg)`;
  }

  fromRotation(rot: Vector2D): number | null {
    const stringd = `${rot.x},${rot.y}`;
    const conversion: { [rotation: string]: number } = {
      '1,0': 0,
      '1,-1': -45,
      '0,-1': -90,
      '-1,-1': -135,
      '-1,0': -180,
      '-1,1': -225,
      '0,1': -270,
      '1,1': -315,
    };
    return conversion[stringd] ?? null;
  }

  isDiagonal(rot: Vector2D) {
    return rot.x !== 0 && rot.y !== 0;
  }

  determineWidth() {
    const rotation = this.rotation();
    const diagonalChange =
      rotation === null || !this.isDiagonal(rotation) ? 1 : Math.sqrt(2);
    return 100 * ((this.length() ?? 1) - 1) * diagonalChange;
  }
}
