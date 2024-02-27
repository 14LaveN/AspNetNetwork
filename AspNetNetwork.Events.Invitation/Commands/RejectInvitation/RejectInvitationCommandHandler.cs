using AspNetNetwork.Application.Core.Abstractions.Common;
using AspNetNetwork.Application.Core.Abstractions.Messaging;
using AspNetNetwork.Database.Common.Abstractions;
using AspNetNetwork.Database.Invitation.Data.Interfaces;
using AspNetNetwork.Domain.Common.Core.Errors;
using AspNetNetwork.Domain.Common.Core.Primitives.Maybe;
using AspNetNetwork.Domain.Common.Core.Primitives.Result;

namespace AspNetNetwork.Events.Invitation.Commands.RejectInvitation;

/// <summary>
/// Represents the <see cref="RejectInvitationCommand"/> class.
/// </summary>
internal sealed class RejectInvitationCommandHandler : ICommandHandler<RejectInvitationCommand, Result>
{
    private readonly IInvitationRepository _invitationRepository;
    private readonly IUnitOfWork<Domain.Identity.Entities.Invitation> _unitOfWork;
    private readonly IDateTime _dateTime;

    /// <summary>
    /// Initializes a new instance of the <see cref="RejectInvitationCommandHandler"/> class.
    /// </summary>
    /// <param name="invitationRepository">The invitation repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="dateTime">The date and time.</param>
    public RejectInvitationCommandHandler(
        IInvitationRepository invitationRepository,
        IUnitOfWork<Domain.Identity.Entities.Invitation> unitOfWork,
        IDateTime dateTime)
    {
        _invitationRepository = invitationRepository;
        _unitOfWork = unitOfWork;
        _dateTime = dateTime;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(RejectInvitationCommand request, CancellationToken cancellationToken)
    {
        Maybe<Domain.Identity.Entities.Invitation> maybeInvitation = await _invitationRepository.GetByIdAsync(request.InvitationId);

        if (maybeInvitation.HasNoValue)
        {
            return Result.Failure(DomainErrors.Invitation.NotFound);
        }

        Domain.Identity.Entities.Invitation invitation = maybeInvitation.Value;

        if (invitation.UserId != request.UserId)
        {
            return Result.Failure(DomainErrors.User.InvalidPermissions);
        }

        Result rejectResult = invitation.Reject(_dateTime.UtcNow);

        if (rejectResult.IsFailure)
        {
            return Result.Failure(rejectResult.Error);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return await Result.Success();
    }
}