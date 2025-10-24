import { Component, inject, ElementRef } from '@angular/core';
import { LoadingSpinnerComponent } from '../shared/loading-spinner.component';
import { CostPipe } from '../shared/pipes/cost.pipe';
import { PositionPipe } from '../shared/pipes/position.pipe';
import { OrderableTableComponent } from '../shared/orderable-table/orderable-table.component';
import { PlayersFiltersComponent, PlayerFilterModel } from './filters/players-filters.component';
import { PaginationComponent } from '../shared/pagination/pagination.component';
import { PAGINATION } from '../shared/constants';
import { ApiDataService } from '../core/services/api-data.service';
import { PlayerPagedDto } from './player-paged.model';
import { PlayerColumnsService } from './player-columns.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-players',
  standalone: true,
  imports: [CommonModule, LoadingSpinnerComponent, CostPipe, OrderableTableComponent, PositionPipe, PlayersFiltersComponent, PaginationComponent],
  templateUrl: './players.component.html',
  styleUrls: ['./players.component.scss']
})
export class PlayersComponent {
  private readonly api = inject(ApiDataService);
  private readonly elRef = inject(ElementRef);
  readonly columnsService = inject(PlayerColumnsService);

  players: PlayerPagedDto[] = [];
  loading = true;
  page = 1;
  pageSize = PAGINATION.DEFAULT_PAGE_SIZE;
  totalPages = 1;
  showColumnToggle = false;
  columnDropdownStyle: any = {};

  orderBy: string = '';
  orderDir: '' | 'asc' | 'desc' = '';

  filterModel: PlayerFilterModel = {};


  fetchPlayers() {
    this.loading = true;
    const request = {
      page: this.page,
      pageSize: this.pageSize,
      ...(this.orderBy && { orderBy: this.orderBy }),
      ...(this.orderDir && { orderDir: this.orderDir }),
      ...this.filterModel
    };
    this.api.getPagedPlayers(request).subscribe({
      next: (players) => {
        this.players = players;
        this.loading = false;
        
        // Dynamically calculate if there are more pages
        // If we get a full page, assume there might be more
        if (players.length === this.pageSize) {
          this.totalPages = this.page + 1; // At least one more page
        } else {
          this.totalPages = this.page; // This is the last page
        }
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  onFilterChange(model: PlayerFilterModel) {
    this.filterModel = model;
    this.page = 1;
    this.fetchPlayers();
  }


  setOrder(orderBy: string, orderDir: 'asc' | 'desc') {
    this.orderBy = orderBy;
    this.orderDir = orderDir;
    this.page = 1;
    this.fetchPlayers();
  }



  toggleColumnToggle() {
    this.showColumnToggle = !this.showColumnToggle;
    if (this.showColumnToggle) {
      setTimeout(() => this.positionColumnDropdown());
    }
  }

  private positionColumnDropdown() {
    const button = this.elRef.nativeElement.querySelector('.column-toggle-btn');
    if (button) {
      const rect = button.getBoundingClientRect();
      this.columnDropdownStyle = {
        top: `${rect.bottom + 4}px`,
        right: `${window.innerWidth - rect.right}px`
      };
    }
  }

  onPageChange(page: number) {
    this.page = page;
    this.fetchPlayers();
  }

  resetFilters(): void {
    this.filterModel = {};
    this.page = 1;
    this.fetchPlayers();
  }
}
