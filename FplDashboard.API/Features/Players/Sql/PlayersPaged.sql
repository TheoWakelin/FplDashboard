SELECT
    p.Id AS PlayerId,
    p.WebName AS PlayerName,
    t.Name AS TeamName,
    p.Position AS Position,
    p.NowCost AS Cost,
    p.Bonus,
    p.TotalPoints,
    p.Minutes,
    p.GoalsScored AS Goals,
    p.Assists,
    p.CleanSheets,
    p.PointsPerGame,
    pgwd.Form,
    pgwd.ExpectedAssistsPer90,
    pgwd.ExpectedGoalInvolvementsPer90,
    pgwd.ExpectedGoalsPer90,
    pgwd.ExpectedGoalsConcededPer90,
    pgwd.DefensiveContributionPer90,
    pgwd.SavesPer90,
    pgwd.SelectedByPercent,
    pgwd.ValueSeason,
    pgwd.ValueForm,
    pgwd.Bps,
    pgwd.Influence,
    pgwd.Creativity,
    pgwd.Threat,
    pgwd.IctIndex
FROM Players p
JOIN Teams t ON p.TeamId = t.Id
JOIN PlayerGameWeekData pgwd ON pgwd.PlayerId = p.Id AND pgwd.GameWeekId = @CurrentGameWeekId
WHERE p.Status = 'a'
  AND (@Position IS NULL OR p.Position = @Position)
    {TeamFilter}
ORDER BY {OrderBy} {OrderDir}
OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
