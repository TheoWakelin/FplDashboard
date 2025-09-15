using FplDashboard.ETL.Interfaces;

namespace FplDashboard.ETL.Services;

public class FplApiClient(HttpClient httpClient) : IFplApiClient
{
    public async Task<string> GetMainFplData(CancellationToken cancellationToken) 
        => await httpClient.GetStringAsync("https://fantasy.premierleague.com/api/bootstrap-static/", cancellationToken);

    public async Task<string> GetFixturesData(CancellationToken cancellationToken)
        => await httpClient.GetStringAsync("https://fantasy.premierleague.com/api/fixtures/", cancellationToken);
}
