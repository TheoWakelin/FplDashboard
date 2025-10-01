import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-sort-arrows',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './sort-arrows.component.html',
  styleUrls: ['./sort-arrows.component.scss']
})
export class SortArrowsComponent {
  @Input() column!: string;
  @Input() orderBy!: string;
  @Input() orderDir: '' | 'asc' | 'desc' = '';
  @Output() sort = new EventEmitter<'asc' | 'desc'>();

  get activeDir(): '' | 'asc' | 'desc' {
    return this.orderBy === this.column ? this.orderDir : '';
  }

  get disabledAsc(): boolean {
    return this.orderBy === this.column && this.orderDir === 'asc';
  }

  get disabledDesc(): boolean {
    return this.orderBy === this.column && this.orderDir === 'desc';
  }

  onSort(dir: 'asc' | 'desc') {
    this.sort.emit(dir);
  }
}
