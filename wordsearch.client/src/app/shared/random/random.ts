/**
 * Random number; INCLUSIVE to max and min
 * @param min
 * @param max
 */
export const randomFloat = (min: number, max: number) => {
  if (min > max) throw new Error('Min is greater than max.');
  const rn = Math.random();
  return rn * (max - min) + min;
};

export const randomInt = (min: number, max: number) => {
  const value = randomFloat(min, max);
  return Math.round(value);
};

export const randomItem = <T>(items: T[]): T => {
  const randIndex = randomInt(0, items.length - 1);
  return items[randIndex];
};
