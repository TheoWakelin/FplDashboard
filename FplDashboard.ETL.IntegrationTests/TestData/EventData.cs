using FplDashboard.DataModel.Models;
using FplDashboard.ETL.Models;

namespace FplDashboard.ETL.IntegrationTests.TestData;

internal static class EventData
{
    internal static readonly Event PreviousGameWeek = new()
    {
        Id = 3,
        IsCurrent = false,
        IsNext = false,
        IsPrevious = true,
        AverageEntryScore = 55,
        HighestScore = 110
    };

    internal static readonly GameWeek CurrentGameWeek = new()
    {
        Id = 4,
        Status = GameWeekStatus.Current,
        AverageEntryScore = 0,
        HighestScore = 0
    };
    
    internal static readonly Event CurrentGameWeekFromEtl = new()
    {
        Id = 4,
        IsCurrent = true,
        IsNext = false,
        IsPrevious = false,
        AverageEntryScore = 0,
        HighestScore = 0
    };
}