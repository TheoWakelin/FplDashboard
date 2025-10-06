using Dapper;
using FplDashboard.API.Features.Dashboard.Models;
using FplDashboard.API.Infrastructure;
using FplDashboard.API.Features.Shared;

namespace FplDashboard.API.Features.Dashboard;

public class DashboardQueries(IDbConnectionFactory connectionFactory, IGeneralQueries generalQueries, ICacheService cacheService)
{
    public async Task<DashboardDataDto> GetDashboardDataAsync(CancellationToken cancellationToken)
    {
        if (cacheService.Get<DashboardDataDto>(CacheKeys.DashboardData) is { } cachedDashboard)
            return cachedDashboard;

        using var connection = connectionFactory.CreateConnection();
        var playerNews = await connection.QueryAsync<PlayerNewsDto>(
            new CommandDefinition(
                """
                SELECT TOP 10 pn.NewsAdded, p.WebName AS PlayerName, t.Name AS TeamName, pn.News
                FROM PlayerNews pn
                JOIN Players p ON pn.PlayerId = p.Id
                JOIN Teams t ON p.TeamId = t.Id
                ORDER BY pn.NewsAdded DESC
                """,
                parameters: null,
                cancellationToken: cancellationToken
            )
        );

        var currentGameWeekId = await generalQueries.GetCurrentGameWeekIdAsync(cancellationToken);

        var topBottomSql = SqlResourceLoader.GetSql("FplDashboard.API.Features.Dashboard.Sql.TopBottomTeamFixtures.sql");
        var topBottomTeams = await connection.QueryAsync<TeamStrengthDto>(
            new CommandDefinition(
                topBottomSql,
                parameters: new { CurrentGameWeekId = currentGameWeekId },
                cancellationToken: cancellationToken
            )
        );

        var dashboardData = new DashboardDataDto
        {
            PlayerNews = playerNews,
            TopTeams = topBottomTeams.Where(t => t.Category == TeamStrengthCategory.Top),
            BottomTeams = topBottomTeams.Where(t => t.Category == TeamStrengthCategory.Bottom),
        };

        cacheService.Set(CacheKeys.DashboardData, dashboardData);
        return dashboardData;
    }
}