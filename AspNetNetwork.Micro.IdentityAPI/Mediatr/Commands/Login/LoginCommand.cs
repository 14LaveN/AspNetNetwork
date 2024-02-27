using AspNetNetwork.Application.ApiHelpers.Responses;
using AspNetNetwork.Application.Core.Abstractions.Messaging;
using AspNetNetwork.Domain.Common.Core.Primitives.Result;
using AspNetNetwork.Domain.Common.ValueObjects;

namespace AspNetNetwork.Micro.IdentityAPI.Mediatr.Commands.Login;

/// <summary>
/// Represents the login command record class.
/// </summary>
/// <param name="UserName">The user name.</param>
/// <param name="Password">The password.</param>
public sealed record LoginCommand(
        string UserName,
        Password Password)
    : ICommand<LoginResponse<Result>>;