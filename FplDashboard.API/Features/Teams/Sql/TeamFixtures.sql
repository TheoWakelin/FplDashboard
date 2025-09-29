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
ORDER BY t.Name, gw.GameWeekNumber

