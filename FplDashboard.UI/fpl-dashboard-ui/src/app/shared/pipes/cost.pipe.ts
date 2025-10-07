import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'cost', standalone: true })
export class CostPipe implements PipeTransform {
  transform(value: number | null | undefined): string {
    if (!value) return '—';
    return `£${(value / 10).toFixed(1)}m`;
  }
}
