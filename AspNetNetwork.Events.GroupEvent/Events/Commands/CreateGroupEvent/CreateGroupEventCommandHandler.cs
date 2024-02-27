using MediatR;
using AspNetNetwork.Application.Core.Abstractions.Common;
using AspNetNetwork.Application.Core.Abstractions.Messaging;
using AspNetNetwork.Database.Common.Abstractions;
using AspNetNetwork.Database.GroupEvent.Data.Interfaces;
using AspNetNetwork.Database.Identity.Data.Interfaces;
using AspNetNetwork.Domain.Common.Core.Errors;
using AspNetNetwork.Domain.Common.Core.Primitives.Maybe;
using AspNetNetwork.Domain.Common.Core.Primitives.Result;
using AspNetNetwork.Domain.Common.ValueObjects;
using AspNetNetwork.Domain.Identity.Entities;
using AspNetNetwork.Domain.Identity.Enumerations;

namespace AspNetNetwork.Events.GroupEvent.Events.Commands.CreateGroupEvent;

/// <summary>
/// Represents the <see cref="CreateGroupEventCommand"/> handler.
/// </summary>
internal sealed class CreateGroupEventCommandHandler : ICommandHandler<CreateGroupEventCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IGroupEventRepository _groupEventRepository;
    private readonly IUnitOfWork<Domain.Identity.Entities.GroupEvent> _unitOfWork;
    private readonly IDateTime _dateTime;
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateGroupEventCommandHandler"/> class.
    /// </summary>
    /// <param name="userRepository">The user repository.</param>
    /// <param name="groupEventRepository">The group event repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="dateTime">The date and time.</param>
    /// <param name="mediator">The mediator.</param>
    public CreateGroupEventCommandHandler(
        IUserRepository userRepository,
        IGroupEventRepository groupEventRepository,
        IUnitOfWork<Domain.Identity.Entities.GroupEvent> unitOfWork,
        IDateTime dateTime,
        IMediator mediator)
    {
        _userRepository = userRepository;
        _groupEventRepository = groupEventRepository;
        _unitOfWork = unitOfWork;
        _dateTime = dateTime;
        _mediator = mediator;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(CreateGroupEventCommand request, CancellationToken cancellationToken)
    {
        if (request.DateTimeUtc <= _dateTime.UtcNow)
        {
            return Result.Failure(DomainErrors.GroupEvent.DateAndTimeIsInThePast);
        }

        Maybe<User> maybeUser = await _userRepository.GetByIdAsync(request.UserId);

        if (maybeUser.HasNoValue)
        {
            return Result.Failure(DomainErrors.User.NotFound);
        }

        Maybe<Category> maybeCategory = Category.FromValue(request.CategoryId);

        if (maybeCategory.HasNoValue)
        {
            return Result.Failure(DomainErrors.Category.NotFound);
        }

        Result<Name> nameResult = Name.Create(request.Name);

        if (nameResult.IsFailure)
        {
            return Result.Failure(nameResult.Error);
        }

        var groupEvent = Domain.Identity.Entities.GroupEvent.Create(maybeUser.Value, nameResult.Value, maybeCategory.Value, request.DateTimeUtc);

        await _groupEventRepository.Insert(groupEvent);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return await Result.Success();
    }
}