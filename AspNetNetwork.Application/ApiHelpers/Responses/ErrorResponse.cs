namespace AspNetNetwork.Application.ApiHelpers.Responses;

/// <summary>
/// Represents the error response class.
/// </summary>
public sealed class ErrorResponse
{
    /// <summary>
    /// Gets or sets message.
    /// </summary>
    public required string Message { get; set; }
    
    /// <summary>
    /// Gets or sets error code.
    /// </summary>
    public required string ErrorCode { get; set; }
}