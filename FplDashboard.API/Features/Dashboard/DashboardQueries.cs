using Dapper;
using FplDashboard.API.Infrastructure;
using FplDashboard.API.Features.Shared;

namespace FplDashboard.API.Features.Dashboard;

public class DashboardQueries(IDbConnectionFactory connectionFactory, IGeneralQueries generalQueries)
{
    public async Task<object> GetDashboardDataAsync(CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var playerNews = await connection.QueryAsync(
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

        var topBottomSql = await File.ReadAllTextAsync("Features/Dashboard/Sql/TopBottomTeamFixtures.sql", cancellationToken);
        var topBottomTeams = await connection.QueryAsync<TeamStrengthDto>(
            new CommandDefinition(
                topBottomSql,
                parameters: new { CurrentGameWeekId = currentGameWeekId },
                cancellationToken: cancellationToken
            )
        );

        return new
        {
            playerNews,
            topTeams = topBottomTeams.Where(t => t.Category == TeamStrengthCategory.Top),
            bottomTeams =  topBottomTeams.Where(t => t.Category == TeamStrengthCategory.Bottom),
        };
    }
}
