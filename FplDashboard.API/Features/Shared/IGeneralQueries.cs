namespace FplDashboard.API.Features.Shared
{
    public interface IGeneralQueries
    {
        Task<int> GetCurrentGameWeekIdAsync(CancellationToken cancellationToken);
    }
}