using Microsoft.AspNetCore.Authorization;

namespace AspNetNetwork.Micro.IdentityAPI;

/// <summary>
/// Represents the require scope authorization handler.
/// </summary>
public sealed class RequireScopeHandler : AuthorizationHandler<ScopeRequirement>
{
    /// <summary>
    /// Handle requirement by authorization handler context.
    /// </summary>
    /// <param name="context">The Authorization Handler Context.</param>
    /// <param name="requirement">The scope requirement.</param>
    /// <returns></returns>
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ScopeRequirement requirement)
    {
        var scopeClaim = context.User.FindFirst(c => c.Type == "scope" && c.Issuer == requirement.Issuer);

        if (scopeClaim == null || string.IsNullOrWhiteSpace(scopeClaim.Value))
        {
            context.Fail(new AuthorizationFailureReason(this, "Scopes was null"));
            return Task.CompletedTask;
        }

        if (scopeClaim.Value.Split(' ').Any(s => s == requirement.Scope))
            context.Succeed(requirement);
        else
            context.Fail(new AuthorizationFailureReason(this, "Scope invalid"));
        
        return Task.CompletedTask;
    }
}

/// <summary>
/// Represents the scope requirement record class.
/// </summary>
/// <param name="Issuer">The issuer.</param>
/// <param name="Scope">The scope.</param>
public sealed record ScopeRequirement(string Issuer, string Scope) : IAuthorizationRequirement
{
    /// <summary>
    /// Gets issuer.
    /// </summary>
    public string Issuer { get; } = Issuer ?? throw new ArgumentNullException(nameof(Issuer));
    
    /// <summary>
    /// Gets scope.
    /// </summary>
    public string Scope { get; } = Scope ?? throw new ArgumentNullException(nameof(Scope));
}