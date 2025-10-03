using Dapper;
using FplDashboard.API.Infrastructure;
using FplDashboard.DataModel.Models;

namespace FplDashboard.API.Features.Shared;

public class GeneralQueries(IDbConnectionFactory connectionFactory, ICacheService cacheService) : IGeneralQueries
{

    public async Task<int> GetCurrentGameWeekIdAsync(CancellationToken cancellationToken)
    {
        if (cacheService.Get<int?>(CacheKeys.CurrentGameWeekId) is { } cachedId)
            return cachedId;

        using var connection = connectionFactory.CreateConnection();
        var id = await connection.QuerySingleAsync<int>(
            new CommandDefinition(
                $"SELECT Id FROM GameWeeks WHERE Status = {(int)GameWeekStatus.Current}",
                parameters: null,
                cancellationToken: cancellationToken
            )
        );
        cacheService.Set(CacheKeys.CurrentGameWeekId, id);
        return id;
    }
}
