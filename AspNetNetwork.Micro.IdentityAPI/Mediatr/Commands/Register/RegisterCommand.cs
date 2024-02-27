using AspNetNetwork.Application.ApiHelpers.Responses;
using AspNetNetwork.Application.Core.Abstractions.Messaging;
using AspNetNetwork.Domain.Common.Core.Primitives.Result;
using AspNetNetwork.Domain.Common.ValueObjects;

namespace AspNetNetwork.Micro.IdentityAPI.Mediatr.Commands.Register;

/// <summary>
/// Represents the register command record class.
/// </summary>
/// <param name="FirstName">The first name.</param>
/// <param name="LastName">The last name.</param>
/// <param name="Email">The email.</param>
/// <param name="Password">The password.</param>
/// <param name="UserName">The user name.</param>
public sealed record RegisterCommand(
    FirstName FirstName,
    LastName LastName,
    EmailAddress Email,
    Password Password,
    string UserName)
    : ICommand<LoginResponse<Result>>;