import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DashboardData } from '../../dashboard/dashboard-data.model';
import { TeamFixturesDto } from '../../teams/team-fixtures.model';
import { environment } from '../../../environments/environment';
import { PlayerPagedDto } from '../../players/player-paged.model';
import { TeamListDto } from '../../teams/team-list.model';

export interface PlayerFilterRequest {
  readonly page?: number;
  readonly pageSize?: number;
  readonly orderBy?: string;
  readonly orderDir?: string;
  readonly teamIds?: readonly number[];
  readonly positionIds?: readonly number[];
  readonly playerName?: string;
  readonly minMinutes?: number;
}

@Injectable({ providedIn: 'root' })
export class ApiDataService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.apiBaseUrl;

  getDashboardData(): Observable<DashboardData> {
    return this.http.get<DashboardData>(`${this.baseUrl}/dashboard/dashboardData`);
  }

  getTeamFixtures(): Observable<TeamFixturesDto[]> {
    return this.http.get<TeamFixturesDto[]>(`${this.baseUrl}/teams/fixtures`);
  }

  getTeamsList(): Observable<TeamListDto[]> {
    return this.http.get<TeamListDto[]>(`${this.baseUrl}/players/teams`);
  }

  getPagedPlayers(filter: PlayerFilterRequest): Observable<PlayerPagedDto[]> {
    return this.http.post<PlayerPagedDto[]>(`${this.baseUrl}/players/paged`, filter);
  }
}
