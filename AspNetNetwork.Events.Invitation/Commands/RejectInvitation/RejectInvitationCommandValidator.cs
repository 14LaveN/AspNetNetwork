using FluentValidation;
using AspNetNetwork.Application.Core.Errors;
using AspNetNetwork.Application.Core.Extensions;

namespace AspNetNetwork.Events.Invitation.Commands.RejectInvitation;

/// <summary>
/// Represents the <see cref="RejectInvitationCommand"/> validator.
/// </summary>
public sealed class RejectInvitationCommandValidator : AbstractValidator<RejectInvitationCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RejectInvitationCommandValidator"/> class.
    /// </summary>
    public RejectInvitationCommandValidator() =>
        RuleFor(x => x.InvitationId)
            .NotEmpty()
            .WithError(ValidationErrors.RejectInvitation.InvitationIdIsRequired);
}