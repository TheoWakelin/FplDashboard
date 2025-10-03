using Dapper;
using FplDashboard.API.Infrastructure;
using FplDashboard.API.Features.Shared;

namespace FplDashboard.API.Features.Teams
{
    public class TeamsQueries(IDbConnectionFactory connectionFactory, IGeneralQueries generalQueries, ICacheService cacheService)
    {
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);

        public async Task<List<TeamFixturesDto>> GetTeamFixturesAsync(CancellationToken cancellationToken)
        {
            if (cacheService.Get<List<TeamFixturesDto>>(CacheKeys.TeamFixtures) is { } cachedFixtures)
                return cachedFixtures;

            using var connection = connectionFactory.CreateConnection();
            var currentGameWeekId = await generalQueries.GetCurrentGameWeekIdAsync(cancellationToken);

            var teamFixturesSql = SqlResourceLoader.GetSql("FplDashboard.API.Features.Teams.Sql.TeamFixtures.sql");
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

            cacheService.Set(CacheKeys.TeamFixtures, teamFixtures, CacheDuration);
            return teamFixtures;
        }
    }
}
