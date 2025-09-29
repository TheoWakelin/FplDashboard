export interface PlayerNews {
  PlayerName: string;
  News: string;
  NewsAdded: string;
  TeamName: string;
}

export interface TeamFixturesDto {
  teamName: string;
  cumulativeStrength: number;
}

export interface DashboardData {
  playerNews: PlayerNews[];
  topTeams: TeamFixturesDto[];
  bottomTeams: TeamFixturesDto[];
}
