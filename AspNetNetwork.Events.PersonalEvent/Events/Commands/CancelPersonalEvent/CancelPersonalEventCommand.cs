using AspNetNetwork.Application.Core.Abstractions.Messaging;
using AspNetNetwork.Domain.Common.Core.Primitives.Result;


namespace AspNetNetwork.Events.PersonalEvent.Events.Commands.CancelPersonalEvent;

/// <summary>
/// Represents the cancel personal event command.
/// </summary>
public sealed class CancelPersonalEventCommand : ICommand<Result>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CancelPersonalEventCommand"/> class.
    /// </summary>
    /// <param name="personalEventId">The personal event identifier.</param>
    /// <param name="userId"></param>
    public CancelPersonalEventCommand(Guid personalEventId,
        Guid userId)
    {
        PersonalEventId = personalEventId;
        UserId = userId;
    }

    /// <summary>
    /// Gets the personal event identifier.
    /// </summary>
    public Guid PersonalEventId { get; }
    
    /// <summary>
    /// Gets the user identifier.
    /// </summary>
    public Guid UserId { get; }
}