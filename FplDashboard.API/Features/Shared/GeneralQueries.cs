using Dapper;
using FplDashboard.API.Infrastructure;
using FplDashboard.DataModel.Models;
using Microsoft.Extensions.Caching.Memory;

namespace FplDashboard.API.Features.Shared;

public class GeneralQueries(IDbConnectionFactory connectionFactory, IMemoryCache memoryCache)
{
    private const string CacheKey = "CurrentGameWeekId";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(12);

    public async Task<int> GetCurrentGameWeekIdAsync(CancellationToken cancellationToken)
    {
        if (memoryCache.TryGetValue(CacheKey, out int cachedId))
            return cachedId;

        using var connection = connectionFactory.CreateConnection();
        var id = await connection.QuerySingleAsync<int>(
            new CommandDefinition(
                $"SELECT Id FROM GameWeeks WHERE Status = {(int)GameWeekStatus.Current}",
                parameters: null,
                cancellationToken: cancellationToken
            )
        );
        memoryCache.Set(CacheKey, id, CacheDuration);
        return id;
    }
}

