import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-multi-select',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './multi-select.component.html',
  styleUrls: ['./multi-select.component.scss']
})
export class MultiSelectComponent<T extends { id: number; name: string }> {
  @Input() items: T[] = [];
  @Input() selectedIds: number[] = [];
  @Input() placeholder: string = 'Select';
  @Input() maxLabelLength: number = 32;
  @Output() selectedIdsChange = new EventEmitter<number[]>();

  open = false;

  getSelectedNames(): string {
    if (!this.selectedIds.length) return this.placeholder;
    const names = this.items.filter(i => this.selectedIds.includes(i.id)).map(i => i.name);
    const label = names.join(', ');
    return label.length > this.maxLabelLength ? label.slice(0, this.maxLabelLength) + '…' : label;
  }

  toggleOpen() {
    this.open = !this.open;
  }

  close() {
    this.open = false;
    this.selectedIdsChange.emit(this.selectedIds);
  }

  toggleSelection(id: number, checked: boolean) {
    if (checked) {
      if (!this.selectedIds.includes(id)) {
        this.selectedIds = [...this.selectedIds, id];
      }
    } else {
      this.selectedIds = this.selectedIds.filter(i => i !== id);
    }
  }
}
