using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AspNetNetwork.Application.Core.Settings.User;

/// <summary>
/// Represents the user info class.
/// </summary>
/// <param name="claims">The claims.</param>
public class UserInfo(ClaimsPrincipal claims)
{
    /// <summary>
    /// The identifier.
    /// </summary>
    public Guid Id { get; set; } = Guid.Parse(claims.FindFirstValue(JwtRegisteredClaimNames.NameId) ?? string.Empty);
    
    /// <summary>
    /// The user name.
    /// </summary>
    public string? UserName { get; set; } = claims.FindFirstValue(JwtRegisteredClaimNames.Name);
    
    /// <summary>
    /// The email.
    /// </summary>
    public string? Email { get; set; } = claims.FindFirstValue(JwtRegisteredClaimNames.Email);
    
    /// <summary>
    /// The first name.
    /// </summary>
    public string? FirstName { get; set; } = claims.FindFirstValue(JwtRegisteredClaimNames.GivenName);
    
    /// <summary>
    /// The last name.
    /// </summary>
    public string? LastName { get; set; } = claims.FindFirstValue(JwtRegisteredClaimNames.FamilyName);
    
    /// <summary>
    /// The patronymic.
    /// </summary>
    public string? Patronymic { get; set; } = claims.FindFirstValue("middle_name");
    
    /// <summary>
    /// The birth date.
    /// </summary>
    public DateTime? BirthDate { get; set; } = DateTime.Parse(claims.FindFirstValue(JwtRegisteredClaimNames.Birthdate) ?? throw new InvalidOperationException());
    
    /// <summary>
    /// The scope.
    /// </summary>
    public string? Scope { get; set; } = claims.FindFirstValue("scope");
}