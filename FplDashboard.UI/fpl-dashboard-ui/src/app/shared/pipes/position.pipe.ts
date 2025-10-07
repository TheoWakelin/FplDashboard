import { Pipe, PipeTransform } from '@angular/core';

export const POSITION_MAP: { [id: number]: string } = {
  1: 'Goalkeeper',
  2: 'Defender',
  3: 'Midfielder',
  4: 'Forward',
};

@Pipe({
  name: 'position',
  standalone: true
})
export class PositionPipe implements PipeTransform {
  transform(value: number | string | null | undefined): string {
    if (!value) return '';
    const id = typeof value === 'string' ? parseInt(value, 10) : value;
    return POSITION_MAP[id] || '';
  }
}
