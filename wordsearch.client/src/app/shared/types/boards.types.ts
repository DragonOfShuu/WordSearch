import Difficulty from './difficulty.type';
import { WordDictionary } from './word-dictionary.types';

export type GameBoard = {
  difficulty: Difficulty;
  boardCharacters: string[][];
  findable: WordDictionary;
  found: string[];
  started: number;
};

export type Board = {
  difficulty: Difficulty;
  boardCharacters: string[][];
  findable: string[];
  found: WordDictionary;
  started: number;
};
