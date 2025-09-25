import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { provideHttpClient } from '@angular/common/http';
import { DashboardComponent } from './dashboard.component';
import { PlayerNewsComponent } from './player-news/player-news.component';
import { TeamFixturesComponent } from './team-fixtures/team-fixtures.component';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    MatTableModule,
    MatCardModule,
    MatIconModule,
    MatProgressSpinnerModule,
    DashboardComponent,
    PlayerNewsComponent,
    TeamFixturesComponent
  ],
  providers: [provideHttpClient()],
  exports: [DashboardComponent]
})
export class DashboardModule { }
