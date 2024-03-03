using AspNetNetwork.Application.Core.Errors;
using AspNetNetwork.Application.Core.Extensions;
using FluentValidation;

namespace AspNetNetwork.Micro.MessagingAPI.Mediatr.Commands.UpdateMessage;

/// <summary>
/// Represents the <see cref="UpdateMessageCommand"/> validator class.
/// </summary>
public sealed class UpdateMessageCommandValidator
    : AbstractValidator<UpdateMessageCommand>
{
    /// <summary>
    /// Validate the <see cref="UpdateMessageCommand"/> class.
    /// </summary>
    public UpdateMessageCommandValidator()
    {
        RuleFor(x =>
                x.Description).NotEqual(string.Empty)
            .WithError(ValidationErrors.CreateMessage.DescriptionIsRequired)
            .MaximumLength(412)
            .WithMessage("Your description too big.");
    }
}