import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { TeamFixturesDto, FixtureScoreDto } from '../dashboard-data.model';

@Component({
  selector: 'app-team-fixtures',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatIconModule, MatTableModule],
  templateUrl: './team-fixtures.component.html',
  styleUrls: ['./team-fixtures.component.scss']
})
export class TeamFixturesComponent implements OnChanges {
  @Input() set teamFixtures(value: TeamFixturesDto[]) {
    this._teamFixtures = value;
    this.dataSource.data = value;
    this.updateGameWeeks();
  }
  get teamFixtures(): TeamFixturesDto[] {
    return this._teamFixtures;
  }
  private _teamFixtures: TeamFixturesDto[] = [];
  public dataSource = new MatTableDataSource<TeamFixturesDto>([]);
  public gameWeeks: number[] = [];
  public displayedColumns: string[] = ['teamName', 'attackingScore', 'defensiveScore', 'overallScore'];
  public fixtureColumns: string[] = [];

  trackByGw(index: number, gw: number): number {
    return gw;
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