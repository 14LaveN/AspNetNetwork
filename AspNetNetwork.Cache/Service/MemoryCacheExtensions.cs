using Microsoft.Extensions.Caching.Memory;

namespace AspNetNetwork.Cache.Service;

/// <summary>
/// Represents the memory cache extenstion static class.
/// </summary>
public static class MemoryCacheExtensions
{
    /// <summary>
    /// Ger or create the object with memory cache.
    /// </summary>
    /// <param name="cache">The memory cache.</param>
    /// <param name="key">The cache key.</param>
    /// <param name="factory">The delegate factory.</param>
    /// <param name="expiration">The expiration time span.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <typeparam name="T">The generic type.</typeparam>
    /// <returns>Return generic object.</returns>
    public static async Task<T> GetOrCreateAsync<T>(
        this IMemoryCache cache,
        string key,
        Func<CancellationToken, Task<T>> factory,
        TimeSpan? expiration = null,
        CancellationToken cancellationToken = default)
    {

        T result = (await cache.GetOrCreateAsync<T>(key, entry =>
        {
            entry.SetAbsoluteExpiration(expiration ?? TimeSpan.FromMinutes(5));

            return factory(cancellationToken);
        }))!;

        return result;
    }
}