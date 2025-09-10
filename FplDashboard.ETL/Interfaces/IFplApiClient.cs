namespace FplDashboard.ETL.Interfaces;

public interface IFplApiClient
{
    Task<string> GetMainFplData(CancellationToken cancellationToken);
}

