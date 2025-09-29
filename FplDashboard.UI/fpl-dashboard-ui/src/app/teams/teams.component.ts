import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { TeamFixturesDto, FixtureScoreDto } from './team-fixtures.model';
import { ApiDataService } from '../api-data.service';
import { OnInit } from '@angular/core';

@Component({
  selector: 'app-teams',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatIconModule, MatTableModule],
  templateUrl: './teams.component.html',
  styleUrls: ['./teams.component.scss']
})
export class TeamsComponent implements OnInit, OnChanges {
  public isLoading = true;
  public error: string | null = null;
  public teamFixtures: TeamFixturesDto[] = [];
  private _teamFixtures: TeamFixturesDto[] = [];
  public dataSource = new MatTableDataSource<TeamFixturesDto>([]);
  public gameWeeks: number[] = [];
  public displayedColumns: string[] = ['teamName', 'attackingScore', 'defensiveScore', 'overallScore'];
  public fixtureColumns: string[] = [];

  constructor(private api: ApiDataService) {}

  ngOnInit(): void {
    this.isLoading = true;
    this.error = null;
    this.api.getTeamFixtures().subscribe({
      next: (data) => {
        this.teamFixtures = data;
        this.dataSource.data = data;
        this.updateGameWeeks();
        this.isLoading = false;
      },
      error: (err) => {
        this.error = 'Failed to load team fixtures.';
        this.isLoading = false;
      }
    });
  }

  getFixtureForGameWeek(team: TeamFixturesDto, gw: number): FixtureScoreDto | undefined {
    return team.fixtures.find(f => f.gameWeekNumber === gw);
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.updateGameWeeks();
  }

  updateGameWeeks() {
    const gwSet = new Set<number>();
    this.teamFixtures.forEach(team => {
      team.fixtures.forEach(fix => gwSet.add(fix.gameWeekNumber));
    });
    this.gameWeeks = Array.from(gwSet).sort((a, b) => a - b);
    // For each gameweek, add a single column
    this.fixtureColumns = [];
    this.gameWeeks.forEach(gw => {
      this.fixtureColumns.push('gw' + gw);
    });
    this.displayedColumns = ['teamName', 'attackingScore', 'defensiveScore', 'overallScore', ...this.fixtureColumns];
  }

  getOpponentDifficultyClass(fixture: FixtureScoreDto): string {
    const totalStrength = fixture.attackingStrength + fixture.defensiveStrength;
    if (totalStrength < -100) {
      return 'difficulty-hard';
    } else if (totalStrength >= -100 && totalStrength <= 100) {
      return 'difficulty-medium';
    } else {
      return 'difficulty-easy';
    }
  }
}

