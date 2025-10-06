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

        var playerNewsTask = FetchPlayerNewsFromDatabase(cancellationToken);
        var fixtureDifficultiesTask = FetchFixtureDifficultiesFromDatabase(cancellationToken);
        
        await Task.WhenAll(playerNewsTask, fixtureDifficultiesTask);

        var dashboardData = new DashboardDataDto
        {
            PlayerNews = playerNewsTask.Result,
            TopTeams = fixtureDifficultiesTask.Result.Where(t => t.Category == TeamStrengthCategory.Top),
            BottomTeams = fixtureDifficultiesTask.Result.Where(t => t.Category == TeamStrengthCategory.Bottom),
        };

        cacheService.Set(CacheKeys.DashboardData, dashboardData);
        return dashboardData;
    }
    
    private async Task<List<PlayerNewsDto>> FetchPlayerNewsFromDatabase(CancellationToken cancellationToken)
    {
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
        return playerNews.ToList();
    }
    
    private async Task<IEnumerable<TeamStrengthDto>> FetchFixtureDifficultiesFromDatabase(CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var currentGameWeekId = await generalQueries.GetCurrentGameWeekIdAsync(cancellationToken);

        var topBottomSql = await SqlResourceLoader.GetSql("FplDashboard.API.Features.Dashboard.Sql.TopBottomTeamFixtures.sql");
        return await connection.QueryAsync<TeamStrengthDto>(
            new CommandDefinition(
                topBottomSql,
                parameters: new { CurrentGameWeekId = currentGameWeekId },
                cancellationToken: cancellationToken
            )
        );
    }
}