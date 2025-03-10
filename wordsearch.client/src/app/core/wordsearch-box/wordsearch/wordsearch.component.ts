import { Component, computed, input, output, signal } from '@angular/core';
import {
  WordDictionary,
  WordType,
} from '../../../shared/types/word-dictionary.types';
import { Vector2D } from '../../../shared/types/vector.types';
import { WordSelectionComponent } from '../word-selection/word-selection.component';
import { KeyValuePipe } from '@angular/common';

@Component({
  selector: 'shuu-wordsearch',
  templateUrl: './wordsearch.component.html',
  styleUrl: './wordsearch.component.sass',
  imports: [WordSelectionComponent, KeyValuePipe],
})
export class WordsearchComponent {
  searchableText = input.required<string[][]>();
  foundText = input.required<WordDictionary>();

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

  selectText = output<WordType>();

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
  #discoverWord(start: Vector2D, rotation: Vector2D, length: number): string {
    let word = '';
    for (let i = 0; i < length; i++) {
      const newX = i * rotation.x + start.x;
      const newY = i * rotation.y + start.y;
      word += this.searchableText()[newY][newX];
    }

    return word;
  }

  getAssumedEndPosition(
    start: Vector2D,
    rotation: Vector2D,
    length: number,
  ): Vector2D {
    const adjustedLength = length - 1;
    return {
      x: start.x + rotation.x * adjustedLength,
      y: start.y + rotation.y * adjustedLength,
    };
  }

  searchClicked() {
    console.log('SEARCH CLICKED');
    const selected = this.selectedTile();
    if (selected === null) return;
    const snappedTo = this.snapToTileData();
    if (snappedTo === null) return;

    const { x, y } = this.getAssumedEndPosition(
      selected,
      snappedTo.rotation,
      snappedTo.length,
    );

    if (
      // True if the selected tile is not horizontal or vertical
      !(selected.x == x || selected.y == y) &&
      // True if the selected tile is not diagonal
      Math.abs(selected.x - x) !== Math.abs(selected.y - y)
    ) {
      console.log('Selection is not perpendicular nor diagonal');
      return;
    }

    if (
      x < 0 ||
      x > this.searchableText()[0].length ||
      y < 0 ||
      y > this.searchableText().length
    ) {
      console.log('Assumed coords not in range');
      return;
    }

    const start: Vector2D = selected;
    const end: Vector2D = { x, y };

    this.findWord(start, end);
  }

  tileClicked(e: MouseEvent, x: number, y: number) {
    const selected = this.selectedTile();
    if (selected === null) {
      this.selectedTile.set({ x, y });
      e.stopImmediatePropagation();
      return;
    }

    if (selected.x === x && selected.y === y) {
      this.selectedTile.set(null);
      e.stopImmediatePropagation();
      return;
    }
  }

  findWord(start: Vector2D, end: Vector2D) {
    const length = this.#findLength(start, end);
    const rotation = this.#findRotation(start, end);
    const word = this.#discoverWord(start, rotation, length);

    const wordInfo = { position: start, rotation, word };
    console.log('Selected Word Info: ', wordInfo);

    this.selectedTile.set(null);
    this.selectText.emit(wordInfo);
  }

  rightClickUnselect() {
    if (this.selectedTile() === null) return true;

    this.selectedTile.set(null);
    return false;
  }

  unSnap() {
    this.snapToTile.set(null);
  }

  snapTo(x: number, y: number) {
    this.snapToTile.set({ x, y });
  }
}
