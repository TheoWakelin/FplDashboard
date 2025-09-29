import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'cost', standalone: true })
export class CostPipe implements PipeTransform {
  transform(value: number): string {
    if (value == null) return '';
    return (value / 10).toFixed(1);
  }
}
