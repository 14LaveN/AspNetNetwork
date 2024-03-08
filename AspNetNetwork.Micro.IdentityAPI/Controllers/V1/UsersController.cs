using AspNetNetwork.Application.ApiHelpers.Contracts;
using AspNetNetwork.Application.ApiHelpers.Infrastructure;
using AspNetNetwork.Application.ApiHelpers.Policy;
using AspNetNetwork.Database.Identity.Data.Interfaces;
using AspNetNetwork.Domain.Common.Core.Errors;
using AspNetNetwork.Domain.Common.Core.Primitives.Result;
using AspNetNetwork.Domain.Common.ValueObjects;
using AspNetNetwork.Micro.IdentityAPI.Contracts.Users.Login;
using AspNetNetwork.Micro.IdentityAPI.Contracts.Users.Register;
using AspNetNetwork.Micro.IdentityAPI.Mediatr.Commands.ChangePassword;
using AspNetNetwork.Micro.IdentityAPI.Mediatr.Commands.Login;
using AspNetNetwork.Micro.IdentityAPI.Mediatr.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AspNetNetwork.Micro.IdentityAPI.Controllers.V1;

/// <summary>
/// Represents the users controller class.
/// </summary>
/// <param name="sender">The sender.</param>
/// <param name="userRepository">The user repository.</param>
[Route("api/v1/users")]
public sealed class UsersController(
        ISender sender,
        IUserRepository userRepository)
    : ApiController(sender, userRepository, nameof(UsersController))
{
    #region Commands.
    
    /// <summary>
    /// Login user.
    /// </summary>
    /// <param name="request">The <see cref="LoginRequest"/> class.</param>
    /// <returns>Base information about login user method.</returns>
    /// <remarks>
    /// Example request:
    /// </remarks>
    /// <response code="200">OK.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost(ApiRoutes.Users.Login)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request) =>
        await Result.Create(request, DomainErrors.General.UnProcessableRequest)
            .Map(loginRequest => new LoginCommand(loginRequest.UserName,Password.Create(loginRequest.Password).Value))
            .Bind(command => BaseRetryPolicy.Policy.Execute(async () =>
                await Sender.Send(command)).Result.Data)
            .Match(Ok, Unauthorized);
    
    /// <summary>
    /// Register user.
    /// </summary>
    /// <param name="request">The <see cref="RegisterRequest"/> class.</param>
    /// <returns>Base information about register user method.</returns>
    /// <remarks>
    /// Example request:
    /// </remarks>
    /// <response code="200">OK.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost(ApiRoutes.Users.Register)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request) =>
        await Result.Create(request, DomainErrors.General.UnProcessableRequest)
            .Map(registerRequest => new RegisterCommand(
                    FirstName.Create(registerRequest.FirstName).Value,
                    LastName.Create(registerRequest.LastName).Value,
                    new EmailAddress(registerRequest.Email),
                    Password.Create(registerRequest.Password).Value,
                    registerRequest.UserName))
            .Bind(command => BaseRetryPolicy.Policy.Execute(async () =>
                await Sender.Send(command)).Result.Data)
            .Match(Ok, Unauthorized);

    /// <summary>
    /// Change password from user.
    /// </summary>
    /// <param name="password">The password.</param>
    /// <returns>Base information about change password from user method.</returns>
    /// <remarks>
    /// Example request:
    /// </remarks>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost(ApiRoutes.Users.ChangePassword)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ChangePassword([FromBody] string password) =>
        await Result.Create(password, DomainErrors.General.UnProcessableRequest)
            .Map(changePasswordRequest => new ChangePasswordCommand(UserId,changePasswordRequest))
            .Bind(command => BaseRetryPolicy.Policy.Execute(async () =>
                await Sender.Send(command)))
            .Match(Ok, BadRequest);
    
    #endregion
}