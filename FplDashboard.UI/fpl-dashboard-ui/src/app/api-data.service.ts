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

        getPagedPlayers(filter: {
            page?: number;
            pageSize?: number;
            orderBy?: string;
            orderDir?: string;
            teamIds?: number[];
            positionIds?: number[];
            playerName?: string;
            minMinutes?: number;
        }): Observable<PlayerPagedDto[]> {
            // Map to backend model
            const body: any = {
                Page: filter.page ?? 1,
                PageSize: filter.pageSize ?? 20,
                OrderBy: filter.orderBy ?? undefined,
                OrderDir: filter.orderDir ?? undefined,
                TeamIds: filter.teamIds?.length ? filter.teamIds : undefined,
                PositionIds: filter.positionIds?.length ? filter.positionIds : undefined,
                PlayerName: filter.playerName ?? undefined,
                MinMinutes: filter.minMinutes ?? undefined
            };
            return this.http.post<PlayerPagedDto[]>(`${this.playersEndpoint}/paged`, body);
        }
}
