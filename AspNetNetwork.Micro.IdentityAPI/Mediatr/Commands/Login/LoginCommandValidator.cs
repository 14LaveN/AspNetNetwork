using FluentValidation;

namespace AspNetNetwork.Micro.IdentityAPI.Mediatr.Commands.Login;

/// <summary>
/// Represents the login command validator class.
/// </summary>
public sealed class LoginCommandValidator
    : AbstractValidator<LoginCommand>
{
    /// <summary>
    /// Validate the login command.
    /// </summary>
    public LoginCommandValidator()
    {
        RuleFor(loginCommand =>
                loginCommand.UserName).NotEqual(string.Empty)
            .WithMessage("You don't enter a user name")
            .MaximumLength(28)
            .WithMessage("Your user name is too big");
        
        RuleFor(loginCommand =>
                loginCommand.Password.Value).NotEqual(string.Empty)
            .WithMessage("You don't enter a password")
            .MaximumLength(36)
            .WithMessage("Your password is too big");
    }
}