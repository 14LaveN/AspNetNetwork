using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace AspNetNetwork.Cache.Service;

/// <summary>
/// Represents the distributed cache extenstion static class.
/// </summary>
public static class DistributedCacheExtensions
{
    /// <summary>
    /// Set the record with distributed cache.
    /// </summary>
    /// <param name="cache">The memory cache.</param>
    /// <param name="recordId">The cache key.</param>
    /// <param name="data">The data.</param>
    /// <param name="absoluteExpireTime">The expiration time span.</param>
    /// <param name="unusedExpireTime">The unused expiration time span.</param>
    /// <typeparam name="T">The generic type.</typeparam>
    public static async Task SetRecordAsync<T>(this IDistributedCache cache,
        string recordId,
        T data,
        TimeSpan? absoluteExpireTime = null,
        TimeSpan? unusedExpireTime = null)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60),
            SlidingExpiration = unusedExpireTime
        };

        var jsonData = JsonSerializer.Serialize(data);
        await cache.SetStringAsync(recordId, jsonData, options);
    }

    /// <summary>
    /// Set the record with distributed cache.
    /// </summary>
    /// <param name="cache">The memory cache.</param>
    /// <param name="recordId">The cache key.</param>
    /// <typeparam name="T">The generic type.</typeparam>
    /// <returns>Return generic object.</returns>
    public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache,
        string recordId)
    {
        var jsonData = await cache.GetStringAsync(recordId);

        return jsonData is null ? default!
            : JsonSerializer.Deserialize<T>(jsonData)!;
    }
}