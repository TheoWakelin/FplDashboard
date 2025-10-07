export interface PlayerNews {
  readonly playerName: string;
  readonly news: string;
  readonly newsAdded: string;
  readonly teamName: string;
}

export interface TeamFixturesDto {
  readonly teamName: string;
  readonly cumulativeStrength: number;
}

export interface DashboardData {
  readonly playerNews: readonly PlayerNews[];
  readonly topTeams: readonly TeamFixturesDto[];
  readonly bottomTeams: readonly TeamFixturesDto[];
}
