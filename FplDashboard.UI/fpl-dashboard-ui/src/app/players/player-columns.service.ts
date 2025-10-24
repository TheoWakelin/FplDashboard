import { Injectable } from '@angular/core';
import { TableHeader } from '../shared/orderable-table/orderable-table.component';

export interface PlayerTableHeader extends TableHeader {
  dataClass?: string;
  pipe?: 'cost' | 'position';
  suffix?: string;
  useFormClass?: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class PlayerColumnsService {
  private readonly defaultHeaders: PlayerTableHeader[] = [
    { key: 'PlayerName', label: 'Player Name', sticky: true, sortable: true, visible: true, dataClass: 'text-align-left' },
    { key: 'TeamName', label: 'Team Name', sticky: true, cssClass: 'second', sortable: true, visible: true, dataClass: 'text-align-left' },
    { key: 'Position', label: 'Position', sortable: true, visible: true, dataClass: 'text-align-left', pipe: 'position' },
    { key: 'Cost', label: 'Cost', sortable: true, visible: true, dataClass: 'text-align-right numeric', pipe: 'cost' },
    { key: 'Bonus', label: 'Bonus', sortable: true, visible: true, dataClass: 'text-align-right numeric' },
    { key: 'TotalPoints', label: 'Total Points', sortable: true, visible: true, dataClass: 'text-align-right numeric' },
    { key: 'Minutes', label: 'Minutes', sortable: true, visible: true, dataClass: 'text-align-right numeric' },
    { key: 'Goals', label: 'Goals', sortable: true, visible: true, dataClass: 'text-align-right numeric' },
    { key: 'Assists', label: 'Assists', sortable: true, visible: true, dataClass: 'text-align-right numeric' },
    { key: 'CleanSheets', label: 'Clean Sheets', sortable: true, visible: true, dataClass: 'text-align-right numeric' },
    { key: 'PointsPerGame', label: 'Points/Game', sortable: true, visible: true, dataClass: 'text-align-right numeric' },
    { key: 'Form', label: 'Form', sortable: true, visible: true, dataClass: 'text-align-right numeric', useFormClass: true },
    { key: 'ExpectedAssistsPer90', label: 'xA/90', sortable: true, visible: true, dataClass: 'text-align-right numeric monospace' },
    { key: 'ExpectedGoalInvolvementsPer90', label: 'xGI/90', sortable: true, visible: true, dataClass: 'text-align-right numeric monospace' },
    { key: 'ExpectedGoalsPer90', label: 'xG/90', sortable: true, visible: true, dataClass: 'text-align-right numeric monospace' },
    { key: 'ExpectedGoalsConcededPer90', label: 'xGC/90', sortable: true, visible: true, dataClass: 'text-align-right numeric monospace' },
    { key: 'DefensiveContributionPer90', label: 'Def. Contrib/90', sortable: true, visible: true, dataClass: 'text-align-right numeric monospace' },
    { key: 'SavesPer90', label: 'Saves/90', sortable: true, visible: true, dataClass: 'text-align-right numeric monospace' },
    { key: 'SelectedByPercent', label: 'Selected %', sortable: true, visible: true, dataClass: 'text-align-right numeric', suffix: '%' },
    { key: 'ValueSeason', label: 'Value Season', sortable: true, visible: true, dataClass: 'text-align-right numeric' },
    { key: 'ValueForm', label: 'Value Form', sortable: true, visible: true, dataClass: 'text-align-right numeric' },
    { key: 'Bps', label: 'BPS', sortable: true, visible: true, dataClass: 'text-align-right numeric' },
    { key: 'Influence', label: 'Influence', sortable: true, visible: true, dataClass: 'text-align-right numeric' },
    { key: 'Creativity', label: 'Creativity', sortable: true, visible: true, dataClass: 'text-align-right numeric' },
    { key: 'Threat', label: 'Threat', sortable: true, visible: true, dataClass: 'text-align-right numeric' },
    { key: 'IctIndex', label: 'ICT Index', sortable: true, visible: true, dataClass: 'text-align-right numeric' }
  ];

  private headers: PlayerTableHeader[] = [];

  constructor() {
    this.resetHeaders();
  }

  getHeaders(): PlayerTableHeader[] {
    return this.headers;
  }

  getVisibleHeaders(): PlayerTableHeader[] {
    return this.headers.filter(h => h.visible !== false);
  }

  toggleColumn(header: PlayerTableHeader): void {
    if (!header.sticky) {
      header.visible = !header.visible;
    }
  }

  resetHeaders(): void {
    this.headers = JSON.parse(JSON.stringify(this.defaultHeaders));
  }

  getColumnValue(player: any, columnKey: string): any {
    const keyMap: { [key: string]: string } = {
      'PlayerName': 'playerName',
      'TeamName': 'teamName',
      'Position': 'position',
      'Cost': 'cost',
      'Bonus': 'bonus',
      'TotalPoints': 'totalPoints',
      'Minutes': 'minutes',
      'Goals': 'goals',
      'Assists': 'assists',
      'CleanSheets': 'cleanSheets',
      'PointsPerGame': 'pointsPerGame',
      'Form': 'form',
      'ExpectedAssistsPer90': 'expectedAssistsPer90',
      'ExpectedGoalInvolvementsPer90': 'expectedGoalInvolvementsPer90',
      'ExpectedGoalsPer90': 'expectedGoalsPer90',
      'ExpectedGoalsConcededPer90': 'expectedGoalsConcededPer90',
      'DefensiveContributionPer90': 'defensiveContributionPer90',
      'SavesPer90': 'savesPer90',
      'SelectedByPercent': 'selectedByPercent',
      'ValueSeason': 'valueSeason',
      'ValueForm': 'valueForm',
      'Bps': 'bps',
      'Influence': 'influence',
      'Creativity': 'creativity',
      'Threat': 'threat',
      'IctIndex': 'ictIndex'
    };

    const propertyKey = keyMap[columnKey];
    return propertyKey ? player[propertyKey] : '';
  }

  getCellClass(header: PlayerTableHeader, player?: any): string {
    const classes: string[] = [];
    
    // Add base data class from header
    if (header.dataClass) {
      classes.push(header.dataClass);
    }
    
    // Add sticky class if needed
    if (header.sticky) {
      classes.push('sticky-col');
    }
    
    // Add cssClass if provided
    if (header.cssClass) {
      classes.push(header.cssClass);
    }
    
    // Add form class if needed
    if (header.useFormClass && player) {
      const formValue = this.getColumnValue(player, header.key);
      classes.push(this.getFormClass(formValue));
    }
    
    return classes.join(' ');
  }

  getFormClass(form: number): string {
    if (form > 7) return 'form-high';
    if (form >= 4) return 'form-mid';
    return 'form-low';
  }
}
