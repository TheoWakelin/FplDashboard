using FplDashboard.ETL.Models;

namespace FplDashboard.ETL.Extensions;

internal static class EtlModelExtensions
{
    public static List<Event> FilterToRelevantEvents(this List<Event> events) =>
        events.Where(e => e.IsCurrent || e.IsNext || e.IsPrevious).ToList();

    public static int GetCurrentGameWeekId(this List<Event> events) => events.Single(e => e.IsCurrent).Id;
}