import {
  Component,
  computed,
  inject,
  input,
  output,
  signal,
} from '@angular/core';
import {
  WordDictionary,
  WordType,
} from '../../../shared/types/word-dictionary.types';
import { Vector2D } from '../../../shared/types/vector.types';
import { WordSelectionComponent } from '../word-selection/word-selection.component';
import { KeyValuePipe, LowerCasePipe } from '@angular/common';
import { WordsearchService } from '../wordsearch-service/wordsearch.service';

@Component({
  selector: 'shuu-wordsearch',
  templateUrl: './wordsearch.component.html',
  styleUrl: './wordsearch.component.sass',
  imports: [WordSelectionComponent, KeyValuePipe, LowerCasePipe],
  providers: [WordsearchService],
})
export class WordsearchComponent {
  wordsearchService = inject(WordsearchService);

  searchableText = input.required<string[][]>();
  foundText = input.required<WordDictionary>();
  pending = input<boolean>(false);

  selectText = output<WordType>();

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
    const selected = this.wordsearchService.selectedTile();
    if (selected === null) return;
    const snappedTo = this.wordsearchService.snapToTileData();
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

    const wordType = this.wordsearchService.findWord(
      start,
      end,
      this.searchableText(),
    );
    this.selectText.emit(wordType);
  }

  tileClicked(e: MouseEvent, x: number, y: number) {
    const selected = this.wordsearchService.selectedTile();
    if (selected === null) {
      this.wordsearchService.selectedTile.set({ x, y });
      e.stopImmediatePropagation();
      return;
    }

    if (selected.x === x && selected.y === y) {
      this.wordsearchService.selectedTile.set(null);
      e.stopImmediatePropagation();
      return;
    }
  }

  rightClickUnselect() {
    if (this.wordsearchService.selectedTile() === null) return true;

    this.wordsearchService.selectedTile.set(null);
    return false;
  }

  unSnap() {
    this.wordsearchService.snapToTile.set(null);
  }

  snapTo(x: number, y: number) {
    this.wordsearchService.snapToTile.set({ x, y });
  }
}
