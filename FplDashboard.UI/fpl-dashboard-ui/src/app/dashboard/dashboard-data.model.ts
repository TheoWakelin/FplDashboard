export interface PlayerNews {
  PlayerName: string;
  News: string;
  NewsAdded: string;
  TeamName: string;
}

export interface FixtureScoreDto {
  teamName: string;
  opponentName: string;
  attackingStrength: number;
  defensiveStrength: number;
  gameWeekNumber: number;
}

export interface TeamFixturesDto {
  teamName: string;
  fixtures: FixtureScoreDto[];
  totalAttackingStrength: number;
  totalDefensiveStrength: number;
  cumulativeStrength: number;
}

export interface DashboardData {
  playerNews: PlayerNews[];
  teamFixtures: TeamFixturesDto[];
}
