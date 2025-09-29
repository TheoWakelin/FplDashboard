import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DashboardData } from './dashboard/dashboard-data.model';
import { TeamFixturesDto } from './teams/team-fixtures.model';
import { environment } from '../environments/environment';
import { PlayerPagedDto } from './players/player-paged.model';

@Injectable({ providedIn: 'root' })
export class ApiDataService {
    private http = inject(HttpClient);
    private readonly apiUrl = `${environment.apiBaseUrl}/dashboard/dashboardData`;
    private readonly teamFixturesUrl = `${environment.apiBaseUrl}/teams/fixtures`;

    private readonly pagedPlayersUrl = `${environment.apiBaseUrl}/players/paged`;

    getDashboardData(): Observable<DashboardData> {
        return this.http.get<DashboardData>(this.apiUrl);
    }

    getTeamFixtures(): Observable<TeamFixturesDto[]> {
        return this.http.get<TeamFixturesDto[]>(this.teamFixturesUrl);
    }

    getPagedPlayers(page: number = 1, pageSize: number = 20, orderBy?: string, orderDir?: string): Observable<PlayerPagedDto[]> {
        const params: any = { page, pageSize };
        if (orderBy) params.orderBy = orderBy;
        if (orderDir) params.orderDir = orderDir;
        return this.http.get<PlayerPagedDto[]>(this.pagedPlayersUrl, { params });
    }
}
