import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DashboardData } from './dashboard/dashboard-data.model';
import { TeamFixturesDto } from './teams/team-fixtures.model';
import { environment } from '../environments/environment';
import { PlayerPagedDto } from './players/player-paged.model';
import { TeamListDto } from './teams/team-list.model';

@Injectable({ providedIn: 'root' })
export class ApiDataService {
    private http = inject(HttpClient);
    private readonly apiUrl = `${environment.apiBaseUrl}/dashboard/dashboardData`;
    private readonly teamFixturesUrl = `${environment.apiBaseUrl}/teams/fixtures`;

    private readonly playersEndpoint = `${environment.apiBaseUrl}/players`;

    getDashboardData(): Observable<DashboardData> {
        return this.http.get<DashboardData>(this.apiUrl);
    }

    getTeamFixtures(): Observable<TeamFixturesDto[]> {
        return this.http.get<TeamFixturesDto[]>(this.teamFixturesUrl);
    }

    getTeamsList(): Observable<TeamListDto[]> {
        return this.http.get<TeamListDto[]>(`${this.playersEndpoint}/teams`);
    }

        getPagedPlayers(
            page: number = 1,
            pageSize: number = 20,
            orderBy?: string,
            orderDir?: string,
            teamIds?: number[]
        ): Observable<PlayerPagedDto[]> {
            const params: any = { page, pageSize };
            if (orderBy) params.orderBy = orderBy;
            if (orderDir) params.orderDir = orderDir;
            if (teamIds && teamIds.length > 0) params.teamIds = teamIds;
            return this.http.get<PlayerPagedDto[]>(`${this.playersEndpoint}/paged`, { params });
        }
}
