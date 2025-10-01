WITH TeamStrengths AS (
    SELECT
        t.Name AS TeamName,
        SUM(
            CASE WHEN f.HomeTeamId = t.Id THEN
                tgd.StrengthAttackHome - oppd.StrengthDefenceAway +
                tgd.StrengthDefenceHome - oppd.StrengthAttackAway
            ELSE
                tgd.StrengthAttackAway - oppd.StrengthDefenceHome +
                tgd.StrengthDefenceAway - oppd.StrengthAttackHome
            END
        ) AS CumulativeStrength
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
    GROUP BY t.Name
),
     TopTeams AS (
         SELECT TOP 5 TeamName, CumulativeStrength, 0 AS Category
         FROM TeamStrengths
         ORDER BY CumulativeStrength DESC
     ),
     BottomTeams AS (
         SELECT TOP 5 TeamName, CumulativeStrength, 1 AS Category
         FROM TeamStrengths
         ORDER BY CumulativeStrength ASC
     )
SELECT * FROM TopTeams
UNION ALL
SELECT * FROM BottomTeams;
