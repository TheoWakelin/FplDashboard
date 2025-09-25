import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardApiService } from './dashboard-api.service';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';

import { DashboardData } from './dashboard-data.model';
import { PlayerNewsComponent } from './player-news/player-news.component';
import { TeamFixturesComponent } from './team-fixtures/team-fixtures.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, MatProgressSpinnerModule, MatIconModule, PlayerNewsComponent, TeamFixturesComponent],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  dashboardData: DashboardData | null = null;
  isLoading = true;
  error: string | null = null;

  constructor(private dashboardApi: DashboardApiService) { }

  ngOnInit(): void {
    this.loadDashboardData();
  }

  private loadDashboardData(): void {
    this.isLoading = true;
    this.error = null;
    this.dashboardApi.getDashboardData().subscribe({
      next: (data) => {
        this.dashboardData = data;
        this.isLoading = false;
      },
      error: (error) => {
        this.error = 'Failed to load dashboard data';
        this.isLoading = false;
        console.error('Error loading dashboard data:', error);
      }
    });
  }
}