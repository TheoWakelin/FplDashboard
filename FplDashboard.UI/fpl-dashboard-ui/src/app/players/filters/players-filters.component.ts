import { Component, EventEmitter, Output, OnInit, inject } from '@angular/core';
import { ApiDataService } from '../../core/services/api-data.service';
import { CommonModule } from '@angular/common';
import { MultiSelectComponent } from '../../shared/multi-select/multi-select.component';
import { POSITION_MAP } from '../../shared/pipes/position.pipe';

export interface PlayerFilterModel {
  teamIds?: number[];
  positionIds?: number[];
  minMinutes?: number;
  playerName?: string;
}

@Component({
  selector: 'app-players-filters',
  standalone: true,
  imports: [CommonModule, MultiSelectComponent],
  templateUrl: './players-filters.component.html',
  styleUrls: ['./players-filters.component.scss']
})
export class PlayersFiltersComponent implements OnInit {
  private readonly api = inject(ApiDataService);
  private lastEmittedFilter?: PlayerFilterModel;
  
  @Output() filterChange = new EventEmitter<PlayerFilterModel>();

  teams: { id: number; name: string }[] = [];
  positionOptions = Object.entries(POSITION_MAP).map(([id, name]) => ({ id: Number(id), name }));

  selectedTeamIds: number[] = [];
  selectedPositions: number[] = [];
  minMinutes: number | null = null;
  playerName: string = '';

  ngOnInit(): void {
    this.api.getTeamsList().subscribe({
      next: (teams) => {
        this.teams = teams;
        this.emitChange();
      },
      error: () => {
        this.emitChange();
      }
    });
  }

  onTeamsSelected(ids: number[]) {
    this.selectedTeamIds = ids;
    this.emitChange();
  }

  onPositionsSelected(ids: number[]) {
    this.selectedPositions = ids;
    this.emitChange();
  }

  onMinMinutesBlur(event: any) {
    const val = Number((event.target as HTMLInputElement).value);
    this.minMinutes = isNaN(val) ? null : val;
    this.emitChange();
  }

  onPlayerNameChange(event: any) {
    this.playerName = (event.target as HTMLInputElement).value;
    this.emitChange();
  }

  onResetFilters() {
    this.selectedTeamIds = [];
    this.selectedPositions = [];
    this.minMinutes = null;
    this.playerName = '';
    this.emitChange();
  }

  private emitChange() {
    const newFilter: PlayerFilterModel = {
      teamIds: this.selectedTeamIds.length ? this.selectedTeamIds : undefined,
      positionIds: this.selectedPositions.length ? this.selectedPositions : undefined,
      minMinutes: this.minMinutes ?? undefined,
      playerName: this.playerName?.trim() ? this.playerName : undefined
    };
    if (!this.isFilterEqual(newFilter, this.lastEmittedFilter)) {
      this.filterChange.emit(newFilter);
      this.lastEmittedFilter = { ...newFilter };
    }
  }

  private isFilterEqual(a?: PlayerFilterModel, b?: PlayerFilterModel): boolean {
    if (!a && !b) return true;
    if (!a || !b) return false;
    return (
      this.arraysEqual(a.teamIds, b.teamIds) &&
      this.arraysEqual(a.positionIds, b.positionIds) &&
      a.minMinutes === b.minMinutes &&
      a.playerName === b.playerName
    );
  }

  private arraysEqual(arr1?: number[], arr2?: number[]): boolean {
    if (!arr1 && !arr2) return true;
    if (!arr1 || !arr2) return false;
    if (arr1.length !== arr2.length) return false;
    return arr1.every((val, idx) => val === arr2[idx]);
  }
}
