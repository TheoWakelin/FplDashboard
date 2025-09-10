using System.Text.Json;
using FplDashboard.ETL.IntegrationTests.TestData;
using FplDashboard.ETL.Models;

namespace FplDashboard.ETL.IntegrationTests.Helpers;

internal static class MockApiResponseHelper
{
    internal static string CreateApiResponse(
        List<Event>? gameWeeks = null,
        List<Team>? teams = null,
        List<PlayerElement>? players = null)
    {
        var wrapper = new WrapperFromApi
        {
            Events = gameWeeks ?? [EventData.CurrentGameWeekFromEtl],
            Teams = teams ?? [],
            Elements = players ?? [],
        };

        return JsonSerializer.Serialize(wrapper);
    }
}
