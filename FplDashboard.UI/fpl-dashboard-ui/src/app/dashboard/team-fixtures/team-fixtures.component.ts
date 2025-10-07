import { Component, Input, inject } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { TeamFixturesDto } from '../dashboard-data.model';

@Component({
  selector: 'app-team-fixtures',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatIconModule],
  templateUrl: './team-fixtures.component.html',
  styleUrls: ['./team-fixtures.component.scss']
})
export class TeamFixturesComponent {
  private readonly router = inject(Router);

  @Input() topTeamFixtures: readonly TeamFixturesDto[] = [];
  @Input() bottomTeamFixtures: readonly TeamFixturesDto[] = [];
  
  goToTeams() {
    this.router.navigate(['/teams']);
  }
}