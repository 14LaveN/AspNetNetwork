using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using AspNetNetwork.Application.Core.Settings.User;
using AspNetNetwork.Domain.Identity.Entities;
using Microsoft.IdentityModel.Tokens;

namespace AspNetNetwork.Micro.IdentityAPI.Extensions;

/// <summary>
/// Represents the json web token extensions class.
/// </summary>
public static class JwtExtensions
{
    /// <summary>
    /// Generate new refresh token by options.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="jwtOptions">The json web token options.</param>
    /// <returns>Returns refresh token.</returns>
    public static (string, DateTime) GenerateRefreshToken(
        this User user,
        JwtOptions jwtOptions)
    {
        var refreshTokenExpireAt = DateTime.UtcNow.AddMinutes(jwtOptions.RefreshTokenExpire);
        var data = new RefreshTokenData
        {
            Expire = refreshTokenExpireAt, 
            UserId = user.Id, 
            Key = StringRandomizer.Randomize()
        };
        
        return (AesEncryptor.Encrypt(jwtOptions.Secret, JsonSerializer.Serialize(data)), refreshTokenExpireAt);
    }
    
    /// <summary>
    /// Generate new access token by options.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="jwtOptions">The json web token options.</param>
    /// <returns>Returns access token.</returns>
    public static string GenerateAccessToken(
        this User user,
        JwtOptions jwtOptions)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret.PadRight(64)));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature);
        if (user.UserName is not null)
        {
            var tokeOptions = new JwtSecurityToken(
                claims: new List<Claim>
                {
                    new (JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                    new (JwtRegisteredClaimNames.Name, user.UserName),
                    new (JwtRegisteredClaimNames.Email, user.EmailAddress),
                    new (JwtRegisteredClaimNames.GivenName, user.Firstname ?? string.Empty),
                    new (JwtRegisteredClaimNames.FamilyName, user.LastName ?? string.Empty),
                },
                expires: DateTime.Now.AddMinutes(jwtOptions.Expire),
                signingCredentials: signinCredentials
            );
            return new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        }

        throw new InvalidOperationException();
    }
}