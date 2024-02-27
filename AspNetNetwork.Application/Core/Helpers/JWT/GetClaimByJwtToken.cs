using System.IdentityModel.Tokens.Jwt;

namespace AspNetNetwork.Application.Core.Helpers.JWT;

/// <summary>
/// Represents the Get claim by jwt token class.
/// </summary>
public static class GetClaimByJwtToken
{
    /// <summary>
    /// Get name by JWT.
    /// </summary>
    /// <param name="token">The JWT.</param>
    /// <returns>Return the name from token.</returns>
    public static string GetNameByToken(string? token)
    {
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        JwtSecurityToken tokenInfo = handler.ReadJwtToken(token);
        
        var claimsPrincipal = tokenInfo.Claims;
        
        var name = claimsPrincipal.FirstOrDefault(x=> x.Type == "name")?.Value;
        return name!;
    }
}