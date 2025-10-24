import { Component, Input, Output, EventEmitter, ElementRef, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-multi-select',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './multi-select.component.html',
  styleUrls: ['./multi-select.component.scss']
})
export class MultiSelectComponent<T extends { id: number; name: string }> {
  private clickListener?: (event: MouseEvent) => void;
  @ViewChild('dropdownDiv', { read: ElementRef }) dropdownDiv?: ElementRef;
  
  constructor(private elRef: ElementRef) {}

  @Input() items: T[] = [];
  @Input() selectedIds: number[] = [];
  @Input() placeholder: string = 'Select';
  @Input() maxLabelLength: number = 32;
  @Output() selectedIdsChange = new EventEmitter<number[]>();

  open = false;
  dropdownStyle: any = {};

    getSelectedNames(): string {
      if (!this.selectedIds.length) return this.placeholder;
      const names = this.items.filter(i => this.selectedIds.includes(i.id)).map(i => i.name);
      const label = names.join(', ');
      return label.length > this.maxLabelLength ? label.slice(0, this.maxLabelLength) + '…' : label;
    }

    toggleOpen() {
      this.open = !this.open;
      if (this.open) {
        setTimeout(() => {
          this.positionDropdown();
          this.clickListener = (event: MouseEvent) => this.handleClickOutside(event);
          document.addEventListener('mousedown', this.clickListener);
        });
      } else {
        this.removeClickListener();
      }
    }

    private positionDropdown() {
      const button = this.elRef.nativeElement.querySelector('button');
      if (button) {
        const rect = button.getBoundingClientRect();
        this.dropdownStyle = {
          top: `${rect.bottom + 4}px`,
          left: `${rect.left}px`
        };
      }
    }

    private handleClickOutside(event: MouseEvent) {
      const target = event.target as HTMLElement;
      if (!this.elRef.nativeElement.contains(target)) {
        this.close();
      }
    }

    private removeClickListener() {
      if (this.clickListener) {
        document.removeEventListener('mousedown', this.clickListener);
        this.clickListener = undefined;
      }
    }

    close() {
      this.open = false;
      this.selectedIdsChange.emit(this.selectedIds);
      this.removeClickListener();
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

    ngOnDestroy() {
      this.removeClickListener();
    }
  }
