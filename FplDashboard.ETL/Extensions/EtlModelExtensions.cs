using FplDashboard.ETL.Models;

namespace FplDashboard.ETL.Extensions;

internal static class EtlModelExtensions
{
    public static int GetCurrentGameWeekNumber(this List<Event> events) => events.Single(e => e.IsCurrent).Id;
}