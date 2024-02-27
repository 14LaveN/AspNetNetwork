using FluentValidation;
using AspNetNetwork.Application.Core.Errors;
using AspNetNetwork.Application.Core.Extensions;

namespace AspNetNetwork.Events.Invitation.Commands.AcceptInvitation;

/// <summary>
/// Represents the <see cref="AcceptInvitationCommand"/> validator.
/// </summary>
public sealed class AcceptInvitationCommandValidator : AbstractValidator<AcceptInvitationCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AcceptInvitationCommandValidator"/> class.
    /// </summary>
    public AcceptInvitationCommandValidator() =>
        RuleFor(x => x.InvitationId)
            .NotEmpty()
            .WithError(ValidationErrors.AcceptInvitation.InvitationIdIsRequired);
}