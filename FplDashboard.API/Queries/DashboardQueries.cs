using Dapper;
using FplDashboard.API.Factories;

namespace FplDashboard.API.Queries;

public class DashboardQueries(IDbConnectionFactory connectionFactory)
{
    public async Task<object> GetDashboardDataAsync()
    {
        using var connection = connectionFactory.CreateConnection();
        var playerNews = await connection.QueryAsync(@"
            SELECT top 10 pn.NewsAdded, p.WebName AS PlayerName, t.Name AS TeamName, pn.News
            FROM PlayerNews pn
            JOIN Players p ON pn.PlayerId = p.Id
            JOIN Teams t ON p.TeamId = t.Id
            ORDER BY pn.NewsAdded DESC
        ");

        // Query for the current gameweek id
        var currentGameWeekId = await connection.QuerySingleAsync<int>(@"
            SELECT Id FROM GameWeeks WHERE Status = 1 -- 1 = Current
        ");

        var teamFixturesRaw = await connection.QueryAsync<FixtureScoreDto>(@"
            SELECT
                t.Name AS TeamName,
                opp.Name AS OpponentName,
                CASE WHEN f.TeamHId = t.Id THEN
                    tgd.StrengthAttackHome - oppd.StrengthDefenceAway
                ELSE
                    tgd.StrengthAttackAway - oppd.StrengthDefenceHome
                END AS AttackingStrength,
                CASE WHEN f.TeamHId = t.Id THEN
                    tgd.StrengthDefenceHome - oppd.StrengthAttackAway
                ELSE
                    tgd.StrengthDefenceAway - oppd.StrengthAttackHome
                END AS DefensiveStrength,
                gw.GameWeekNumber
            FROM Teams t
            JOIN Fixtures f ON f.TeamHId = t.Id OR f.TeamAId = t.Id
            JOIN GameWeeks gw ON f.EventId = gw.Id
            JOIN Teams opp ON
                (f.TeamHId = t.Id AND f.TeamAId = opp.Id) OR
                (f.TeamAId = t.Id AND f.TeamHId = opp.Id)
            JOIN TeamGameWeekData tgd ON tgd.TeamId = t.Id AND tgd.GameWeekId = @CurrentGameWeekId
            JOIN TeamGameWeekData oppd ON oppd.TeamId = opp.Id AND oppd.GameWeekId = @CurrentGameWeekId
            WHERE f.EventId IN (
                SELECT TOP 5 Id FROM GameWeeks g WHERE g.Id > @CurrentGameWeekId ORDER BY GameWeekNumber ASC
            )
        ", new { CurrentGameWeekId = currentGameWeekId });

        var teamFixtures = teamFixturesRaw
            .GroupBy(f => f.TeamName)
            .Select(g => new TeamFixturesDto
            {
                TeamName = g.Key,
                Fixtures = g.ToList()
            })
            .OrderByDescending(t => t.CumulativeStrength)
            .ToList();

        var topPlayers = await connection.QueryAsync(@"
            SELECT top 20 * FROM Players ORDER BY PointsPerGame DESC
        ");

        return new
        {
            playerNews,
            teamFixtures,
            topPlayers
        };
    }
}
