import { Routes } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { PlayersComponent } from './players/players.component';
import { TeamsComponent } from './teams/teams.component';

export const routes: Routes = [
	{ path: '', component: DashboardComponent },
	{ path: 'players', component: PlayersComponent },
	{ path: 'teams', component: TeamsComponent },
];
