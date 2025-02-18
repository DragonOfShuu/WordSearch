import { Vector2D } from './vector.types';

type Difficulty = {
  level: number;
  intensity: 'medium';
  /** Optional, because size is often determined 
   * by the intensity relative to the level from
   * the server */
  size?: Vector2D;
  time: number;
};

export default Difficulty;
