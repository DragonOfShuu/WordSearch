import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'rgbify',
})
export class RgbPipePipe implements PipeTransform {
  transform(
    value: { r: string | number; g: string | number; b: string | number },
    ...args: unknown[]
  ): unknown {
    const { r, g, b } = value;
    return `rgb(${r}, ${g}, ${b})`;
  }
}
