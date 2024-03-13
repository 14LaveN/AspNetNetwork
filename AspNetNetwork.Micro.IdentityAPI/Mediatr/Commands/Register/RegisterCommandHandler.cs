using System.Security.Authentication;
using AspNetNetwork.Application.ApiHelpers.Responses;
using AspNetNetwork.Application.Core.Abstractions.Messaging;
using AspNetNetwork.Domain.Common.Core.Exceptions;
using AspNetNetwork.Domain.Common.Core.Primitives.Result;
using AspNetNetwork.Domain.Common.Enumerations;
using AspNetNetwork.Domain.Common.ValueObjects;
using AspNetNetwork.Domain.Identity.Entities;
using AspNetNetwork.Micro.IdentityAPI.Mediatr.Commands.Login;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AspNetNetwork.Micro.IdentityAPI.Mediatr.Commands.Register;

/// <summary>
/// Represents the register command handler class.
/// </summary>
/// <param name="logger">The logger.</param>
/// <param name="userManager">The user manager.</param>
/// <param name="sender">The sender.</param>
/// <param name="signInManager">The sign in manager.</param>
public sealed class RegisterCommandHandler(ILogger<RegisterCommandHandler> logger,
        UserManager<User> userManager,
        ISender sender,
        SignInManager<User> signInManager)
    : ICommandHandler<RegisterCommand, LoginResponse<Result>>
{
    private readonly SignInManager<User> _signInManager = signInManager ?? throw new ArgumentNullException();
 
    /// <inheritdoc />
    public async Task<LoginResponse<Result>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation($"Request for login an account - {request.UserName} {request.LastName}");
            
            Result<FirstName> firstNameResult = FirstName.Create(request.FirstName);
            Result<LastName> lastNameResult = LastName.Create(request.LastName);
            Result<EmailAddress> emailResult = EmailAddress.Create(request.Email);
            Result<Password> passwordResult = Password.Create(request.Password);
            
            var user = await userManager.FindByNameAsync(request.UserName);

            if (user is not null)
            {
                logger.LogWarning("User with the same name already taken");
                throw new NotFoundException(nameof(user), "User with the same name");
            }

            user = User.Create(firstNameResult.Value, lastNameResult.Value, emailResult.Value, passwordResult.Value);
            
            var result = await userManager.CreateAsync(user, request.Password);
            
            LoginResponse<Result> loginResponse = new LoginResponse<Result>
            {
                Description = "",
                StatusCode = StatusCode.TaskIsHasAlready
            };

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                loginResponse = await sender.Send(new LoginCommand(request.UserName,request.Password), cancellationToken);

                logger.LogInformation($"User authorized - {user.UserName} {DateTime.UtcNow}");
            }
            return new LoginResponse<Result>
            {
                Description = "Register account",
                StatusCode = StatusCode.Ok,
                Data = Result.Success(),
                AccessToken = loginResponse.AccessToken, 
                RefreshToken = loginResponse.RefreshToken,
                RefreshTokenExpireAt = loginResponse.RefreshTokenExpireAt
            };
        }
        catch (Exception exception)
        {
            logger.LogError(exception, $"[RegisterCommandHandler]: {exception.Message}");
            throw new AuthenticationException(exception.Message);
        }
    }
}