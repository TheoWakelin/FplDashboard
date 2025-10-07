import { Pipe, PipeTransform } from '@angular/core';
import { formatDistanceToNow } from 'date-fns';

@Pipe({
  name: 'humanizeDate',
  standalone: true
})
export class HumanizeDatePipe implements PipeTransform {
  transform(value: string | Date | null | undefined): string {
    if (!value) return '—';
    return formatDistanceToNow(new Date(value), { addSuffix: true });
  }
}
