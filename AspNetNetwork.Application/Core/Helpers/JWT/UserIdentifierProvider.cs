using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using AspNetNetwork.Application.Core.Abstractions.Helpers.JWT;

namespace AspNetNetwork.Application.Core.Helpers.JWT;

/// <summary>
/// Represents the user identifier provider.
/// </summary>
public sealed class UserIdentifierProvider : IUserIdentifierProvider
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserIdentifierProvider"/> class.
    /// </summary>
    /// <param name="httpContextAccessor">The HTTP context accessor.</param>
    public UserIdentifierProvider(IHttpContextAccessor httpContextAccessor)
    {
        Claim userIdClaim = httpContextAccessor.HttpContext?.User?.FindFirst("nameId")
                            ?? throw new ArgumentException("The user identifier claim is required.", nameof(httpContextAccessor));
        
        UserId = new Guid(userIdClaim.Value);
    }

    /// <inheritdoc />
    public Guid UserId { get; }
}