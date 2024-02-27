using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ErrorResponse = AspNetNetwork.Application.ApiHelpers.Responses.ErrorResponse;

namespace AspNetNetwork.Application.ApiHelpers.ExceptionHandler;

/// <summary>
/// Represents the global exception handler class.
/// </summary>
/// <param name="logger">The logger.</param>
public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    : IExceptionHandler
{
    /// <inheritdoc />
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "An unhandled exception occurred.");

        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        httpContext.Response.ContentType = "application/json";

        var errorResponse = new ErrorResponse()
        {
            Message = "An error occurred. Please try again later.",
            ErrorCode = "500"
        };

        var json = JsonSerializer.Serialize(errorResponse);
        await httpContext.Response.WriteAsJsonAsync(json, cancellationToken);

        return true;
    }
}