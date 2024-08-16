using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace UrlShortenerApi.Infrastructure.Cache;

public static class DistributedCacheExtensions
{
    private static readonly SemaphoreSlim Semaphore = new(1, 1);

    public static DistributedCacheEntryOptions DefaultExpiration => new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
    };

    public static async Task<T?> GetOrCreateAsync<T>(
        this IDistributedCache cache,
        string key,
        Func<CancellationToken, Task<T>> factory,
        DistributedCacheEntryOptions? cacheOptions = null,
        CancellationToken token = default)
    {
        T? data;

        var cachedData = await cache.GetStringAsync(key, token);

        if (!string.IsNullOrWhiteSpace(cachedData))
        {
            data = JsonSerializer.Deserialize<T>(cachedData);

            if (data is not null)
            {
                return data;
            }
        }
        
        await Semaphore.WaitAsync(token);
        try
        {
            cachedData = await cache.GetStringAsync(key, token);

            if (!string.IsNullOrWhiteSpace(cachedData))
            {
                data = JsonSerializer.Deserialize<T>(cachedData);

                if (data is not null)
                {
                    return data;
                }
            }

            data = await factory(token);

            await cache.SetStringAsync(
                key,
                JsonSerializer.Serialize(data),
                cacheOptions ?? DefaultExpiration,
                token);
        }
        finally
        {
            Semaphore.Release();
        }

        return data;
    }
}