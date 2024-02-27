using AspNetNetwork.Application.Core.Abstractions.Common;
using AspNetNetwork.Application.Core.Abstractions.Messaging;
using AspNetNetwork.Database.Common.Abstractions;
using AspNetNetwork.Database.GroupEvent.Data.Interfaces;
using AspNetNetwork.Domain.Common.Core.Errors;
using AspNetNetwork.Domain.Common.Core.Primitives.Maybe;
using AspNetNetwork.Domain.Common.Core.Primitives.Result;
using AspNetNetwork.Domain.Common.ValueObjects;

namespace AspNetNetwork.Events.GroupEvent.Events.Commands.UpdateGroupEvent;

/// <summary>
/// Represents the <see cref="UpdateGroupEventCommand"/> handler.
/// </summary>
public sealed class UpdateGroupEventCommandHandler : ICommandHandler<UpdateGroupEventCommand, Result>
{
    private readonly IGroupEventRepository _groupEventRepository;
    private readonly IUnitOfWork<Domain.Identity.Entities.GroupEvent> _unitOfWork;
    private readonly IDateTime _dateTime;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateGroupEventCommandHandler"/> class.
    /// </summary>
    /// <param name="groupEventRepository">The group event repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="dateTime">The date and time.</param>
    public UpdateGroupEventCommandHandler(
        IGroupEventRepository groupEventRepository,
        IUnitOfWork<Domain.Identity.Entities.GroupEvent> unitOfWork,
        IDateTime dateTime)
    {
        _groupEventRepository = groupEventRepository;
        _unitOfWork = unitOfWork;
        _dateTime = dateTime;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(UpdateGroupEventCommand request, CancellationToken cancellationToken)
    {
        if (request.DateTimeUtc <= _dateTime.UtcNow)
        {
            return Result.Failure(DomainErrors.GroupEvent.DateAndTimeIsInThePast);
        }

        Maybe<Domain.Identity.Entities.GroupEvent> maybeGroupEvent = await _groupEventRepository.GetByIdAsync(request.GroupEventId);

        if (maybeGroupEvent.HasNoValue)
        {
            return Result.Failure(DomainErrors.GroupEvent.NotFound);
        }

        Domain.Identity.Entities.GroupEvent groupEvent = maybeGroupEvent.Value;

        if (groupEvent.UserId != request.UserId)
        {
            return Result.Failure(DomainErrors.User.InvalidPermissions);
        }

        Result<Name> nameResult = Name.Create(request.Name);

        if (nameResult.IsFailure)
        {
            return Result.Failure(nameResult.Error);
        }

        groupEvent.ChangeName(nameResult.Value);

        groupEvent.ChangeDateAndTime(request.DateTimeUtc);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
            
        return await Result.Success();
    }
}