import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoadingSpinnerComponent } from '../shared/loading-spinner.component';
import { CostPipe } from '../shared/pipes/cost.pipe';

import { ApiDataService } from '../api-data.service';
import { PlayerPagedDto } from './player-paged.model';

@Component({
  selector: 'app-players',
  standalone: true,
  imports: [CommonModule, LoadingSpinnerComponent, CostPipe],
  templateUrl: './players.component.html',
  styleUrls: ['./players.component.scss']
})
export class PlayersComponent {
  players: PlayerPagedDto[] = [];
  loading = false;
  page = 1;
  pageSize = 20;

  constructor(private api: ApiDataService) {}

  ngOnInit() {
    this.fetchPlayers();
  }

  fetchPlayers() {
    this.loading = true;
    this.api.getPagedPlayers(this.page, this.pageSize).subscribe({
      next: (data) => {
        this.players = data;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }
}
