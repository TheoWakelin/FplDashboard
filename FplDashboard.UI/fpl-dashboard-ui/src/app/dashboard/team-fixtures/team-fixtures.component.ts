import { Component, Input } from '@angular/core';
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
  @Input() topTeamFixtures: TeamFixturesDto[] = [];
  @Input() bottomTeamFixtures: TeamFixturesDto[] = [];
  constructor(private router: Router) {}
  goToTeams() {
    this.router.navigate(['/teams']);
  }
}