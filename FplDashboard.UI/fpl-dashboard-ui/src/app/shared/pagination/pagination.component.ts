import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-pagination',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.scss']
})
export class PaginationComponent {
  @Input() currentPage = 1;
  @Input() totalPages = 1;
  @Input() disabled = false;
  
  @Output() pageChange = new EventEmitter<number>();

  get pageNumbers(): number[] {
    const pages: number[] = [];
    const maxVisible = 5;
    let start = Math.max(1, this.currentPage - Math.floor(maxVisible / 2));
    let end = Math.min(this.totalPages, start + maxVisible - 1);
    
    if (end - start < maxVisible - 1) {
      start = Math.max(1, end - maxVisible + 1);
    }
    
    for (let i = start; i <= end; i++) {
      pages.push(i);
    }
    return pages;
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages && !this.disabled && page !== this.currentPage) {
      this.pageChange.emit(page);
    }
  }

  prevPage(): void {
    if (this.currentPage > 1 && !this.disabled) {
      this.pageChange.emit(this.currentPage - 1);
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages && !this.disabled) {
      this.pageChange.emit(this.currentPage + 1);
    }
  }
}
