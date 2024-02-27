using AspNetNetwork.Domain.Common.ValueObjects;
using AspNetNetwork.Micro.IdentityAPI.Mediatr.Commands.Register;

namespace AspNetNetwork.Micro.IdentityAPI.Contracts.Users.Register;

/// <summary>
/// Represents the register request record class.
/// </summary>
/// <param name="FirstName">The first name.</param>
/// <param name="LastName">The last name.</param>
/// <param name="Email">The email.</param>
/// <param name="Password">The password.</param>
/// <param name="UserName">The user name.</param>
public sealed record RegisterRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string UserName);