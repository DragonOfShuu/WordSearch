import { Component, computed, input, output, signal } from '@angular/core';
import {
  WordDictionary,
  WordType,
} from '../../../shared/types/word-dictionary.types';
import { Vector2D } from '../../../shared/types/vector.types';
import { WordSelectionComponent } from "../word-selection/word-selection.component";

@Component({
  selector: 'shuu-wordsearch',
  templateUrl: './wordsearch.component.html',
  styleUrl: './wordsearch.component.sass',
  imports: [WordSelectionComponent],
})
export class WordsearchComponent {
  searchableText = input.required<string[][]>();
  foundText = input.required<WordDictionary>();
  foundCoords = computed(
    () => {
      const newFound: {[x: string]: WordType[]} = {}
      const words = Object.values(this.foundText());
      
      words.forEach((word) => {
        const coords = `${word.position.x},${word.position.y}`
        if (newFound[coords] !== undefined) {
          newFound[coords].push(word)
          return;
        }

        newFound[coords] = [word];
      })

      return newFound;
    }
  )

  selectedTile = signal<Vector2D|null>(null)

  selectText = output<WordType>();

  #findLength(start: Vector2D, end: Vector2D): number {
    const xLength = Math.abs(start.x - end.x)
    const yLength = Math.abs(start.y - end.y)

    return (xLength || yLength) + 1;
  }

  #find1DRotation(start: number, end: number) {
    return Math.min(Math.max(end-start, -1), 1)
  }

  #findRotation(start: Vector2D, end: Vector2D): Vector2D {
    return { x: this.#find1DRotation(start.x, end.x), y: this.#find1DRotation(start.y, end.y) }
  }

  #findWord(start: Vector2D, rotation: Vector2D, length: number): string {
    let word = ""
    for (let i = 0; i < length; i++) {
      const newX = (i * rotation.x) + start.x
      const newY = (i * rotation.y) + start.y
      word += this.searchableText()[newY][newX]
    }

    return word;
  }

  tileClicked(x: number, y: number) {
    const selected = this.selectedTile()
    if (selected === null) {
      this.selectedTile.set({x, y});
      return;
    }

    if (
      selected.x === x
      && selected.y === y
    ) {
      this.selectedTile.set(null);
      return;
    }

    const start: Vector2D = selected;
    const end: Vector2D = {x, y};

    const length = this.#findLength(start, end);
    console.log("Length: ", length)
    const rotation = this.#findRotation(start, end);
    const word = this.#findWord(start, rotation, length);

    const wordInfo = { position: start, rotation, word }
    console.log("Selected Word Info: ", wordInfo)

    this.selectedTile.set(null)
    this.selectText.emit(wordInfo)
  }
}
