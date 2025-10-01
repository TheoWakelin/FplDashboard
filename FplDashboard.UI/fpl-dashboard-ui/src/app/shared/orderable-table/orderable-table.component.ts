import { Component, Input, Output, EventEmitter, ContentChild, TemplateRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SortArrowsComponent } from '../sort-arrows/sort-arrows.component';

export interface TableHeader {
  key: string;
  label: string;
  sortable?: boolean;
  sticky?: boolean;
  cssClass?: string;
}

@Component({
  selector: 'app-orderable-table',
  standalone: true,
  imports: [CommonModule, SortArrowsComponent],
  templateUrl: './orderable-table.component.html',
  styleUrls: ['./orderable-table.component.scss']
})
export class OrderableTableComponent {
  @Input() headers: TableHeader[] = [];
  @Input() orderBy: string = '';
  @Input() orderDir: '' | 'asc' | 'desc' = '';
  @Output() sort = new EventEmitter<{column: string, direction: 'asc' | 'desc'}>();

  @ContentChild(TemplateRef) bodyTemplate!: TemplateRef<any>;

  onSort(column: string, direction: 'asc' | 'desc') {
    this.sort.emit({ column, direction });
  }
}
