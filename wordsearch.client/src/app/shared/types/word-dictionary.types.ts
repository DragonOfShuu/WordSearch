import { Vector2D } from './vector.types';

export type WordDictionary = {
  [word: string]: WordType;
};

export type WordType = {
  word: string;
  position: Vector2D;
  rotation: Vector2D;
};
