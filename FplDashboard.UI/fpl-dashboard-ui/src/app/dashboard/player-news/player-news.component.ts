import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { PlayerNews } from '../dashboard-data.model';
import { HumanizeDatePipe } from '../../shared/pipes/humanize-date.pipe';

@Component({
  selector: 'app-player-news',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatCardModule, MatIconModule, HumanizeDatePipe],
  templateUrl: './player-news.component.html',
  styleUrls: ['./player-news.component.scss']
})
export class PlayerNewsComponent {
  @Input() playerNews: readonly PlayerNews[] = [];
  displayedColumns: string[] = ['playerImage', 'playerName', 'teamName', 'news', 'newsAdded'];
}