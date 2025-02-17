import { Board } from './boards.types';

export type FindWordResults = {
  board: Board;
  xpGain: number;
  wordsFound: string[];
};
