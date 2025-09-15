using FplDashboard.DataModel.Models;
using FixtureFromEtl = FplDashboard.ETL.Models.Fixture;

namespace FplDashboard.ETL.IntegrationTests.TestData;

public static class FixtureData
{
    public static List<Fixture> Gameweek38 =>
    [
        new() { EventId = EventData.FinalGameWeek.Id, TeamAId = TeamData.ArsenalFromEtl.Id, TeamHId = TeamData.CrystalPalaceFromEtl.Id, Finished = false, TeamAScore = null, TeamHScore = null },
    ];

    public static List<Fixture> InitialGameweek4 =>
    [
        new() { EventId = EventData.CurrentGameWeekFromEtl.Id, TeamAId = TeamData.ArsenalFromEtl.Id, TeamHId = TeamData.CrystalPalaceFromEtl.Id, Finished = false, TeamAScore = null, TeamHScore = null },
    ];
    
    public static List<FixtureFromEtl> Gameweek38FromEtl =>
        [
            new() { GameweekId = EventData.FinalGameWeek.Id, TeamAId = TeamData.ArsenalFromEtl.Id, TeamHId = TeamData.CrystalPalaceFromEtl.Id, Finished = false, TeamAScore = null, TeamHScore = null },
        ];

    public static List<FixtureFromEtl> InitialGameweek4FromEtl =>
        [
            new() { GameweekId = EventData.CurrentGameWeekFromEtl.Id, TeamAId = TeamData.ArsenalFromEtl.Id, TeamHId = TeamData.CrystalPalaceFromEtl.Id, Finished = false, TeamAScore = null, TeamHScore = null },
        ];

    public static List<FixtureFromEtl> UpdatedGameweek4FromEtl =>
    [
        new() { GameweekId = EventData.CurrentGameWeekFromEtl.Id, TeamAId = TeamData.ArsenalFromEtl.Id, TeamHId = TeamData.CrystalPalaceFromEtl.Id, Finished = true, TeamAScore = 2, TeamHScore = 1 },
    ];
}

