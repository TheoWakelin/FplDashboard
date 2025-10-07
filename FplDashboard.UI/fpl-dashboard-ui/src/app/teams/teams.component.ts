import { Component, Input, OnChanges, SimpleChanges, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoadingSpinnerComponent } from '../shared/loading-spinner.component';
import { TeamFixturesDto, FixtureScoreDto } from './team-fixtures.model';
import { ApiDataService } from '../core/services/api-data.service';

@Component({
  selector: 'app-teams',
  standalone: true,
  imports: [CommonModule, LoadingSpinnerComponent],
  templateUrl: './teams.component.html',
  styleUrls: ['./teams.component.scss']
})
export class TeamsComponent implements OnInit, OnChanges {
  private readonly api = inject(ApiDataService);

  public isLoading = true;
  public error: string | null = null;
  public teamFixtures: TeamFixturesDto[] = [];
  public gameWeeks: number[] = [];

  ngOnInit(): void {
    this.isLoading = true;
    this.error = null;
    this.api.getTeamFixtures().subscribe({
      next: (data) => {
        this.teamFixtures = data;
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

