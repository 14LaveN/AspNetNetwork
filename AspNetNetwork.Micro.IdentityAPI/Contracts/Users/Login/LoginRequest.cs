namespace AspNetNetwork.Micro.IdentityAPI.Contracts.Users.Login;

/// <summary>
/// Represents the login request record class.
/// </summary>
/// <param name="UserName">The user name.</param>
/// <param name="Password">The password</param>
public sealed record LoginRequest(
    string UserName,
    string Password);