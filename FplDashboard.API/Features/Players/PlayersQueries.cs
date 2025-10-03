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
        PlayerFilterRequest request,
        CancellationToken cancellationToken)
    {
        var currentGameWeekId = await generalQueries.GetCurrentGameWeekIdAsync(cancellationToken);
        var sql = SqlResourceLoader.GetSql("FplDashboard.API.Features.Players.Sql.PlayersPaged.sql");

        // Sanitize orderBy
        var orderByColumn = AllowedOrderColumns.ContainsKey(request.OrderBy ?? "") ? AllowedOrderColumns[request.OrderBy!] : "p.TotalPoints";
        var orderDirection = (request.OrderDir?.ToUpper() == "ASC") ? "ASC" : "DESC";
        var secondaryOrder = orderByColumn == "p.WebName" ? "" : ", p.WebName";

        // Build filters
        var teamFilter = BuildInFilter(request.TeamIds, "p.TeamId", "TeamIds");
        var positionFilter = BuildInFilter(request.PositionIds, "p.Position", "PositionIds");

        // Replace placeholders in SQL
        sql = sql.Replace("{OrderBy}", orderByColumn)
            .Replace("{OrderDir}", orderDirection)
            .Replace("{TeamFilter}", teamFilter)
            .Replace("{PositionFilter}", positionFilter)
            .Replace("{SecondaryOrder}", secondaryOrder);

        var offset = (request.Page - 1) * request.PageSize;

        var players = await connectionFactory.CreateConnection().QueryAsync<PlayerPagedDto>(
            new CommandDefinition(
                sql,
                parameters: new
                {
                    TeamIds = request.TeamIds,
                    PositionIds = request.PositionIds,
                    PlayerName = request.PlayerName,
                    Offset = offset,
                    PageSize = request.PageSize,
                    CurrentGameWeekId = currentGameWeekId,
                    MinMinutes = request.MinMinutes
                },
                cancellationToken: cancellationToken
            )
        );
        return players.ToList();
    }

    private static string BuildInFilter(List<int>? ids, string column, string paramName)
    {
        return ids is { Count: > 0 } ? $"AND {column} IN @{paramName}" : "";
    }

    public async Task<List<TeamListDto>> GetAllTeamsAsync(CancellationToken cancellationToken)
    {
        const string sql = "SELECT Id, Name FROM Teams ORDER BY Name";
        var teams = await connectionFactory.CreateConnection().QueryAsync<TeamListDto>(
            new CommandDefinition(sql, cancellationToken: cancellationToken)
        );
        return teams.ToList();
    }
}
