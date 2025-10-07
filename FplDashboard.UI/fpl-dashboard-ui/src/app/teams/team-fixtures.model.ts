export interface FixtureScoreDto {
  readonly teamName: string;
  readonly opponentName: string;
  readonly attackingStrength: number;
  readonly defensiveStrength: number;
  readonly gameWeekNumber: number;
}

export interface TeamFixturesDto {
  readonly teamName: string;
  readonly fixtures: readonly FixtureScoreDto[];
  readonly totalAttackingStrength: number;
  readonly totalDefensiveStrength: number;
  readonly cumulativeStrength: number;
}
