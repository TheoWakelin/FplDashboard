SELECT
    t.Name AS TeamName,
    opp.Name AS OpponentName,
    CASE WHEN f.HomeTeamId = t.Id THEN
        tgd.StrengthAttackHome - oppd.StrengthDefenceAway
    ELSE
        tgd.StrengthAttackAway - oppd.StrengthDefenceHome
    END AS AttackingStrength,
    CASE WHEN f.HomeTeamId = t.Id THEN
        tgd.StrengthDefenceHome - oppd.StrengthAttackAway
    ELSE
        tgd.StrengthDefenceAway - oppd.StrengthAttackHome
    END AS DefensiveStrength,
    gw.GameWeekNumber
FROM Teams t
JOIN Fixtures f ON f.HomeTeamId = t.Id OR f.AwayTeamId = t.Id
JOIN GameWeeks gw ON f.GameweekId = gw.Id
JOIN Teams opp ON
    (f.HomeTeamId = t.Id AND f.AwayTeamId = opp.Id) OR
    (f.AwayTeamId = t.Id AND f.HomeTeamId = opp.Id)
JOIN TeamGameWeekData tgd ON tgd.TeamId = t.Id AND tgd.GameWeekId = @CurrentGameWeekId
JOIN TeamGameWeekData oppd ON oppd.TeamId = opp.Id AND oppd.GameWeekId = @CurrentGameWeekId
WHERE f.GameweekId IN (
    SELECT TOP 5 Id FROM GameWeeks g WHERE g.Id > @CurrentGameWeekId ORDER BY GameWeekNumber ASC
)

