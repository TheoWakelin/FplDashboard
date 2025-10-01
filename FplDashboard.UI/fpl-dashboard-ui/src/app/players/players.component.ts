import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoadingSpinnerComponent } from '../shared/loading-spinner.component';
import { CostPipe } from '../shared/pipes/cost.pipe';
import { OrderableTableComponent, TableHeader } from '../shared/orderable-table/orderable-table.component';

import { ApiDataService } from '../api-data.service';
import { PlayerPagedDto } from './player-paged.model';

@Component({
  selector: 'app-players',
  standalone: true,
  imports: [CommonModule, LoadingSpinnerComponent, CostPipe, OrderableTableComponent],
  templateUrl: './players.component.html',
  styleUrls: ['./players.component.scss']
})
export class PlayersComponent {
  players: PlayerPagedDto[] = [];
  loading = false;
  page = 1;
  pageSize = 20;

  orderBy: string = '';
  orderDir: '' | 'asc' | 'desc' = '';

  tableHeaders: TableHeader[] = [
    { key: 'PlayerName', label: 'Player Name', sticky: true, sortable: true },
    { key: 'TeamName', label: 'Team Name', sticky: true, cssClass: 'second', sortable: true },
    { key: 'Position', label: 'Position', sortable: true },
    { key: 'Cost', label: 'Cost', sortable: true },
    { key: 'Bonus', label: 'Bonus', sortable: true },
    { key: 'TotalPoints', label: 'Total Points', sortable: true },
    { key: 'Minutes', label: 'Minutes', sortable: true },
    { key: 'Goals', label: 'Goals', sortable: true },
    { key: 'Assists', label: 'Assists', sortable: true },
    { key: 'CleanSheets', label: 'Clean Sheets', sortable: true },
    { key: 'PointsPerGame', label: 'Points/Game', sortable: true },
    { key: 'Form', label: 'Form', sortable: true },
    { key: 'ExpectedAssistsPer90', label: 'xA/90', sortable: true },
    { key: 'ExpectedGoalInvolvementsPer90', label: 'xGI/90', sortable: true },
    { key: 'ExpectedGoalsPer90', label: 'xG/90', sortable: true },
    { key: 'ExpectedGoalsConcededPer90', label: 'xGC/90', sortable: true },
    { key: 'DefensiveContributionPer90', label: 'Def. Contrib/90', sortable: true },
    { key: 'SavesPer90', label: 'Saves/90', sortable: true },
    { key: 'SelectedByPercent', label: 'Selected %', sortable: true },
    { key: 'ValueSeason', label: 'Value Season', sortable: true },
    { key: 'ValueForm', label: 'Value Form', sortable: true },
    { key: 'Bps', label: 'BPS', sortable: true },
    { key: 'Influence', label: 'Influence', sortable: true },
    { key: 'Creativity', label: 'Creativity', sortable: true },
    { key: 'Threat', label: 'Threat', sortable: true },
    { key: 'IctIndex', label: 'ICT Index', sortable: true }
  ];

  constructor(private api: ApiDataService) {}

  ngOnInit() {
    this.fetchPlayers();
  }

  fetchPlayers() {
    this.loading = true;
    this.api.getPagedPlayers(this.page, this.pageSize, this.orderBy, this.orderDir).subscribe({
      next: (data) => {
        this.players = data;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  setOrder(orderBy: string, orderDir: 'asc' | 'desc') {
    this.orderBy = orderBy;
    this.orderDir = orderDir;
    this.page = 1;
    this.fetchPlayers();
  }

  prevPage() {
    if (this.page > 1 && !this.loading) {
      this.page--;
      this.fetchPlayers();
    }
  }

  nextPage() {
    if (!this.loading) {
      this.page++;
      this.fetchPlayers();
    }
  }
}
