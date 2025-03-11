import { computed, Injectable, signal } from '@angular/core';
import { Vector2D } from '../../../shared/types/vector.types';

@Injectable()
export class WordsearchService {
  selectedTile = signal<Vector2D | null>(null);
  snapToTile = signal<Vector2D | null>(null);
  snapToTileData = computed<{
    position: Vector2D;
    rotation: Vector2D;
    length: number;
  } | null>(() => {
    const selected = this.selectedTile();
    const snapTo = this.snapToTile();

    if (selected == null || snapTo == null) return null;

    return {
      position: snapTo,
      rotation: this.#findRotation(selected, snapTo),
      length: this.#findLength(selected, snapTo),
    };
  });

  /**
   * Find the amount of tiles that make
   * the distance from start to end [INCLUSIVE
   * TO START AND END]
   * @param start
   * @param end
   * @returns
   */
  #findLength(start: Vector2D, end: Vector2D): number {
    const xLength = Math.abs(start.x - end.x);
    const yLength = Math.abs(start.y - end.y);

    return (xLength || yLength) + 1;
  }

  /**
   * Generate a normalized rotation based on
   * the start and end.
   * @param start
   * @param end
   * @returns
   */
  #find1DRotation(start: number, end: number) {
    return Math.min(Math.max(end - start, -1), 1);
  }

  #findRotation(start: Vector2D, end: Vector2D): Vector2D {
    return {
      x: this.#find1DRotation(start.x, end.x),
      y: this.#find1DRotation(start.y, end.y),
    };
  }

  /**
   * Gets a word off the board, being provided the starting location
   * and the rotation and length to follow to collect the word.
   * @param start The starting location of where we collected the word
   * @param rotation The rotation we need to take to make it to the word
   * @param length The length of the word
   * @returns The new ord
   */
  #discoverWord(
    start: Vector2D,
    rotation: Vector2D,
    length: number,
    searchable: string[][],
  ): string {
    let word = '';
    for (let i = 0; i < length; i++) {
      const newX = i * rotation.x + start.x;
      const newY = i * rotation.y + start.y;
      word += searchable[newY][newX];
    }

    return word;
  }

  findWord(start: Vector2D, end: Vector2D, searchable: string[][]) {
    const length = this.#findLength(start, end);
    const rotation = this.#findRotation(start, end);
    const word = this.#discoverWord(start, rotation, length, searchable);

    const wordInfo = { position: start, rotation, word };
    console.log('Selected Word Info: ', wordInfo);

    this.selectedTile.set(null);
    return wordInfo;
  }

  constructor() {}
}
