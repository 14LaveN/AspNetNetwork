using AspNetNetwork.Application.Core.Abstractions.Messaging;
using AspNetNetwork.Domain.Common.Core.Primitives.Result;


namespace AspNetNetwork.Events.PersonalEvent.Events.Commands.UpdatePersonalEvent;

/// <summary>
/// Represents the update personal event command.
/// </summary>
public sealed class UpdatePersonalEventCommand : ICommand<Result>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdatePersonalEventCommand"/> class.
    /// </summary>
    /// <param name="personalEventId">The personal event identifier.</param>
    /// <param name="name">The event name.</param>
    /// <param name="dateTimeUtc">The date and time of the event in UTC format.</param>
    /// <param name="userId"></param>
    public UpdatePersonalEventCommand(
        Guid personalEventId,
        string name,
        DateTime dateTimeUtc,
        Guid userId)
    {
        PersonalEventId = personalEventId;
        Name = name;
        UserId = userId;
        DateTimeUtc = dateTimeUtc.ToUniversalTime();
    }

    /// <summary>
    /// Gets the personal event identifier.
    /// </summary>
    public Guid PersonalEventId { get; }

    /// <summary>
    /// Gets the user identifier.
    /// </summary>
    public Guid UserId { get; }
    
    /// <summary>
    /// Gets the name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the date and time in UTC format.
    /// </summary>
    public DateTime DateTimeUtc { get; }
}