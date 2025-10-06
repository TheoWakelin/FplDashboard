using Microsoft.Extensions.Caching.Memory;

namespace FplDashboard.API.Features.Shared
{
    public class MemoryCacheService(IMemoryCache cache) : ICacheService
    {
        private static readonly TimeSpan DefaultDuration = TimeSpan.FromHours(12);

        public T? Get<T>(string key)
        {
            if (cache.TryGetValue(key, out var value) && value is T typedValue)
                return typedValue;
            return default;
        }

        public void Set<T>(string key, T value)
        {
            cache.Set(key, value, DefaultDuration);
        }
    }
}
