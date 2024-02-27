using FluentValidation;

namespace AspNetNetwork.Micro.IdentityAPI.Mediatr.Commands.Register;

/// <summary>
/// Represents the register command validator class.
/// </summary>
internal class RegisterCommandValidator
    : AbstractValidator<RegisterCommand>
{
    /// <summary>
    /// Validate the login command.
    /// </summary>
    public RegisterCommandValidator()
    {
        RuleFor(registerCommand =>
                registerCommand.UserName).NotEqual(string.Empty)
            .WithMessage("You don't enter a user name")
            .MaximumLength(28)
            .WithMessage("Your user name is too big");
        
        RuleFor(registerCommand =>
                registerCommand.Password.Value).NotEqual(string.Empty)
            .WithMessage("You don't enter a password")
            .MaximumLength(36)
            .WithMessage("Your password is too big");

        RuleFor(registerCommand =>
                registerCommand.Email.Value).NotEqual(string.Empty)
            .WithMessage("You don't enter a password")
            .EmailAddress();
        
        RuleFor(registerCommand =>
                registerCommand.FirstName.Value).NotEqual(string.Empty)
            .WithMessage("You don't enter a first name")
            .MaximumLength(36)
            .WithMessage("Your first name is too big");
        
        RuleFor(registerCommand =>
                registerCommand.LastName.Value).NotEqual(string.Empty)
            .WithMessage("You don't enter a last name")
            .MaximumLength(36)
            .WithMessage("Your last name is too big");
    }
}