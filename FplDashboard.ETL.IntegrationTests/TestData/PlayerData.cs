using FplDashboard.DataModel.Models;
using FplDashboard.ETL.Models;

namespace FplDashboard.ETL.IntegrationTests.TestData;

internal static class PlayerData
{
    internal static readonly Player Saka = new() { Id = 1, WebName = "Saka", TeamId = TeamData.Arsenal.Id, NowCost = 100};
    internal static readonly Player Eze = new() { Id = 2, WebName = "Eze", TeamId = TeamData.CrystalPalace.Id, NowCost = 70};
    
    internal static readonly PlayerElement SakaFromEtl = new() { Id = 1, WebName = "Saka", Team = TeamData.ArsenalFromEtl.Id, NowCost = 110, EventPoints = 15};
    internal static readonly PlayerElement EzeTransferred = new() { Id = 2, WebName = "Eze", Team = TeamData.Arsenal.Id, NowCost = 75, EventPoints = 15 };
}