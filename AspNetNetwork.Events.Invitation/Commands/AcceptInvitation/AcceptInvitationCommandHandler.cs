using MediatR;
using AspNetNetwork.Application.Core.Abstractions.Common;
using AspNetNetwork.Application.Core.Abstractions.Messaging;
using AspNetNetwork.Database.Common.Abstractions;
using AspNetNetwork.Database.Invitation.Data.Interfaces;
using AspNetNetwork.Domain.Common.Core.Errors;
using AspNetNetwork.Domain.Common.Core.Primitives.Maybe;
using AspNetNetwork.Domain.Common.Core.Primitives.Result;

namespace AspNetNetwork.Events.Invitation.Commands.AcceptInvitation;

/// <summary>
/// Represents the <see cref="AcceptInvitationCommand"/> class.
/// </summary>
internal sealed class AcceptInvitationCommandHandler : ICommandHandler<AcceptInvitationCommand, Result>
{
    private readonly IInvitationRepository _invitationRepository;
    private readonly IUnitOfWork<Domain.Identity.Entities.Invitation> _unitOfWork;
    private readonly IDateTime _dateTime;
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="AcceptInvitationCommandHandler"/> class.
    /// </summary>
    /// <param name="invitationRepository">The invitation repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="dateTime">The date and time.</param>
    /// <param name="mediator">The mediator.</param>
    public AcceptInvitationCommandHandler(
        IInvitationRepository invitationRepository,
        IUnitOfWork<Domain.Identity.Entities.Invitation> unitOfWork,
        IDateTime dateTime,
        IMediator mediator)
    {
        _invitationRepository = invitationRepository;
        _unitOfWork = unitOfWork;
        _dateTime = dateTime;
        _mediator = mediator;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(AcceptInvitationCommand request, CancellationToken cancellationToken)
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

        Result result = invitation.Accept(_dateTime.UtcNow);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return await Result.Success();
    }
}