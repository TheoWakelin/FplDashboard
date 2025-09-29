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
