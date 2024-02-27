using AspNetNetwork.Application.Core.Abstractions.Messaging;
using AspNetNetwork.Domain.Common.Core.Primitives.Result;

namespace AspNetNetwork.Events.GroupEvent.Events.Commands.UpdateGroupEvent;

/// <summary>
/// Represents the update group event command.
/// </summary>
public sealed class UpdateGroupEventCommand : ICommand<Result>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateGroupEventCommand"/> class.
    /// </summary>
    /// <param name="groupEventId">The group event identifier.</param>
    /// <param name="name">The event name.</param>
    /// <param name="dateTimeUtc">The date and time of the event in UTC format.</param>
    /// <param name="userId">The user identifier.</param>
    public UpdateGroupEventCommand(
        Guid groupEventId,
        string name,
        DateTime dateTimeUtc,
        Guid userId)
    {
        GroupEventId = groupEventId;
        Name = name;
        UserId = userId;
        DateTimeUtc = dateTimeUtc.ToUniversalTime();
    }

    /// <summary>
    /// Gets the group event identifier.
    /// </summary>
    public Guid GroupEventId { get; }

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