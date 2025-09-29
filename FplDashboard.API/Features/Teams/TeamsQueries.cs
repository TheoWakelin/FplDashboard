using Dapper;
using FplDashboard.API.Infrastructure;

namespace FplDashboard.API.Features.Teams
{
    public class TeamsQueries(IDbConnectionFactory connectionFactory)
    {
        public async Task<List<TeamFixturesDto>> GetTeamFixturesAsync(CancellationToken cancellationToken)
        {
            using var connection = connectionFactory.CreateConnection();
            var currentGameWeekId = await connection.QuerySingleAsync<int>(
                new CommandDefinition(
                    "SELECT Id FROM GameWeeks WHERE Status = 1 -- 1 = Current",
                    parameters: null,
                    cancellationToken: cancellationToken
                )
            );

            var teamFixturesSql = await File.ReadAllTextAsync("Features/Teams/Sql/TeamFixtures.sql", cancellationToken);
            var fixturesRaw = await connection.QueryAsync<FixtureScoreDto>(
                new CommandDefinition(
                    teamFixturesSql,
                    parameters: new { CurrentGameWeekId = currentGameWeekId },
                    cancellationToken: cancellationToken
                )
            );

            var teamFixtures = fixturesRaw
                .GroupBy(f => f.TeamName)
                .Select(g => new TeamFixturesDto
                {
                    TeamName = g.Key,
                    Fixtures = g.ToList()
                })
                .OrderByDescending(t => t.CumulativeStrength)
                .ToList();

            return teamFixtures;
        }
    }
}
