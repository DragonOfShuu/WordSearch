import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'round',
})
export class RoundPipe implements PipeTransform {
  transform(value: number, ...args: ['floor' | 'ceil' | 'round'] | []): number {
    const operationType = args[0] ?? 'round';

    switch (operationType) {
      case 'ceil':
        return Math.ceil(value);
      case 'floor':
        return Math.floor(value);
      case 'round':
        return Math.round(value);
    }
  }
}
