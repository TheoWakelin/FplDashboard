namespace FplDashboard.ETL.Helpers;

public static class YearHelpers
{
    public static int GetYearCurrentSeasonStarted()
    {
        var now = DateTime.UtcNow;
        return now.Month >= 8 ? now.Year : now.Year - 1;
    }
}