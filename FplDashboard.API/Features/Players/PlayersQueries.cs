using Dapper;
using FplDashboard.API.Features.Shared;
using FplDashboard.API.Infrastructure;

namespace FplDashboard.API.Features.Players;

public class PlayersQueries(IDbConnectionFactory connectionFactory, IGeneralQueries generalQueries)
{
    private static readonly Dictionary<string, string> AllowedOrderColumns = new()
    {
        { "PlayerName", "p.WebName" },
        { "TeamName", "t.Name" },
        { "Position", "p.Position" },
        { "Cost", "p.NowCost" },
        { "Bonus", "p.Bonus" },
        { "TotalPoints", "p.TotalPoints" },
        { "Minutes", "p.Minutes" },
        { "Goals", "p.GoalsScored" },
        { "Assists", "p.Assists" },
        { "CleanSheets", "p.CleanSheets" },
        { "PointsPerGame", "p.PointsPerGame" },
        { "Form", "pgwd.Form" },
        { "ExpectedAssistsPer90", "pgwd.ExpectedAssistsPer90" },
        { "ExpectedGoalInvolvementsPer90", "pgwd.ExpectedGoalInvolvementsPer90" },
        { "ExpectedGoalsPer90", "pgwd.ExpectedGoalsPer90" },
        { "ExpectedGoalsConcededPer90", "pgwd.ExpectedGoalsConcededPer90" },
        { "DefensiveContributionPer90", "pgwd.DefensiveContributionPer90" },
        { "SavesPer90", "pgwd.SavesPer90" },
        { "SelectedByPercent", "pgwd.SelectedByPercent" },
        { "ValueSeason", "pgwd.ValueSeason" },
        { "ValueForm", "pgwd.ValueForm" },
        { "Bps", "pgwd.Bps" },
        { "Influence", "pgwd.Influence" },
        { "Creativity", "pgwd.Creativity" },
        { "Threat", "pgwd.Threat" },
        { "IctIndex", "pgwd.IctIndex" }
    };

    public async Task<List<PlayerPagedDto>> GetPagedPlayersAsync(
        int? teamId,
        string? position,
        string? orderBy,
        string? orderDir,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var currentGameWeekId = await generalQueries.GetCurrentGameWeekIdAsync(cancellationToken);
        var sql = await File.ReadAllTextAsync("Features/Players/Sql/PlayersPaged.sql", cancellationToken);

        // Sanitize orderBy
        var orderByColumn = AllowedOrderColumns.ContainsKey(orderBy ?? "") ? AllowedOrderColumns[orderBy!] : "p.TotalPoints";
        var orderDirection = (orderDir?.ToUpper() == "ASC") ? "ASC" : "DESC";

        // Replace placeholders in SQL
        sql = sql.Replace("{OrderBy}", orderByColumn).Replace("{OrderDir}", orderDirection);

        var offset = (page - 1) * pageSize;

        var players = await connectionFactory.CreateConnection().QueryAsync<PlayerPagedDto>(
            new CommandDefinition(
                sql,
                parameters: new
                {
                    TeamId = teamId,
                    Position = position,
                    Offset = offset,
                    PageSize = pageSize,
                    CurrentGameWeekId = currentGameWeekId
                },
                cancellationToken: cancellationToken
            )
        );
        return players.ToList();
    }
}
