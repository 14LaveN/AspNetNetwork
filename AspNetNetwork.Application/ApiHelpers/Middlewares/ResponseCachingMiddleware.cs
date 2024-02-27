using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;

namespace AspNetNetwork.Application.ApiHelpers.Middlewares;

/// <summary>
/// Represents the response caching middleware class.
/// </summary>
/// <param name="next">The next request delegate.</param>
/// <param name="cache">The distributed cache interface.</param>
public sealed class ResponseCachingMiddleware(
    RequestDelegate next,
    IDistributedCache cache)
{
    /// <summary>
    /// Invoke middleware. 
    /// </summary>
    /// <param name="context">The http context.</param>
    public async Task Invoke(HttpContext context)
    {
        var cacheKey = context.Request.Path;
        var cachedResponse = await cache.GetStringAsync(cacheKey);

        if (!cachedResponse.IsNullOrEmpty())
        {
            await context.Response.WriteAsync(cachedResponse!);
            return;
        }

        var originalResponseBody = context.Response.Body;
        using var memStream = new MemoryStream();
        context.Response.Body = memStream;

        await next(context);

        memStream.Position = 0;
        var responseBody = await new StreamReader(memStream).ReadToEndAsync();

        await cache.SetStringAsync(cacheKey, responseBody);

        memStream.Position = 0;
        await memStream.CopyToAsync(originalResponseBody);
    }
}