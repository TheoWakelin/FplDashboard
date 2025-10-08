using FplDashboard.DataModel.Models;

namespace FplDashboard.ETL.Extensions;

internal static class GameWeeksExtensions
{
    public static int GetIdOrDefault(this GameWeek[] gameWeek, int gameWeekNumber) => gameWeek.FirstOrDefault(gw => gw.GameWeekNumber == gameWeekNumber)?.Id ?? gameWeekNumber;
}