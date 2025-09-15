using FplDashboard.ETL.Models;

namespace FplDashboard.ETL.Extensions;

internal static class EtlModelExtensions
{
    public static int GetCurrentGameWeekId(this List<Event> events) => events.Single(e => e.IsCurrent).Id;
    public static int GetPreviousGameWeekId(this List<Event> events) => events.SingleOrDefault(e => e.IsPrevious)?.Id ?? 0;
}