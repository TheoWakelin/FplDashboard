using Dapper;
using FplDashboard.API.Infrastructure;

namespace FplDashboard.API.Features.Dashboard;

public class DashboardQueries(IDbConnectionFactory connectionFactory)
{
    public async Task<object> GetDashboardDataAsync(CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var playerNews = await connection.QueryAsync(
            new CommandDefinition(
                """
                SELECT top 10 pn.NewsAdded, p.WebName AS PlayerName, t.Name AS TeamName, pn.News
                                FROM PlayerNews pn
                                JOIN Players p ON pn.PlayerId = p.Id
                                JOIN Teams t ON p.TeamId = t.Id
                                ORDER BY pn.NewsAdded DESC
                """,
                parameters: null,
                cancellationToken: cancellationToken
            )
        );

        // Query for the current gameweek id
        var currentGameWeekId = await connection.QuerySingleAsync<int>(
            new CommandDefinition(
                @"SELECT Id FROM GameWeeks WHERE Status = 1 -- 1 = Current",
                parameters: null,
                cancellationToken: cancellationToken
            )
        );

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
