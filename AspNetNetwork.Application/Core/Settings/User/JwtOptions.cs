namespace AspNetNetwork.Application.Core.Settings.User;

/// <summary>
/// Represents jwt options class.
/// </summary>
public sealed class JwtOptions
{
    /// <summary>
    /// Gets or sets secret.
    /// </summary>
    public string Secret { get; set; } = null!;

    /// <summary>
    /// Gets or sets string list valid audiences.
    /// </summary>
    public List<string> ValidAudiences { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets string list valid issuers.
    /// </summary>
    public List<string> ValidIssuers { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets expire.
    /// </summary>
    public int Expire { get; set; } = 3600;
    
    /// <summary>
    /// Gets or sets refresh token expire.
    /// </summary>
    public int RefreshTokenExpire { get; set; } = 20160;
}