using FplDashboard.DataModel.Models;
using TeamFromEtl = FplDashboard.ETL.Models.Team;

namespace FplDashboard.ETL.IntegrationTests.TestData;

internal static class TeamData
{
    internal static readonly Team Arsenal = new()
        {
            Id = 1,
            Name = "Arsenal",
            ShortName = "ARS"
        };

    internal static readonly TeamFromEtl ArsenalFromEtl = new()
        {
            Id = Arsenal.Id,
            Name = Arsenal.Name,
            ShortName = Arsenal.ShortName
        };

    internal static readonly Team CrystalPalace = new()
    {
        Id = 2,
        Name = "Crystal Palace",
        ShortName = "CRY"
    };

    internal static readonly TeamFromEtl CrystalPalaceFromEtl = new()
    {
        Id = CrystalPalace.Id,
        Name = CrystalPalace.Name,
        ShortName = CrystalPalace.ShortName
    };
}