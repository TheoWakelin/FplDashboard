import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DashboardData } from './dashboard/dashboard-data.model';
import { TeamFixturesDto } from './teams/team-fixtures.model';
import { environment } from '../environments/environment';

@Injectable({ providedIn: 'root' })
export class ApiDataService {
  private http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiBaseUrl}/dashboard/dashboardData`;
  private readonly teamFixturesUrl = `${environment.apiBaseUrl}/teams/fixtures`;

  getDashboardData(): Observable<DashboardData> {
    return this.http.get<DashboardData>(this.apiUrl);
  }

  getTeamFixtures(): Observable<TeamFixturesDto[]> {
    return this.http.get<TeamFixturesDto[]>(this.teamFixturesUrl);
  }
}
