using AspNetNetwork.Domain.Common.Core.Primitives.Maybe;
using AspNetNetwork.Domain.Entities;
using AspNetNetwork.Domain.Identity.Entities;

namespace AspNetNetwork.Database.GroupEvent.Data.Interfaces;

/// <summary>
/// Represents the group event repository interface.
/// </summary>
public interface IGroupEventRepository
{
    /// <summary>
    /// Gets the group event with the specified identifier.
    /// </summary>
    /// <param name="groupEventId">The group event identifier.</param>
    /// <returns>The maybe instance that may contain the group event with the specified identifier.</returns>
    Task<Maybe<Domain.Identity.Entities.GroupEvent>> GetByIdAsync(Guid groupEventId);

    /// <summary>
    /// Inserts the specified group event to the database.
    /// </summary>
    /// <param name="groupEvent">The group event to be inserted to the database.</param>
    Task Insert(Domain.Identity.Entities.GroupEvent groupEvent);
    
    /// <summary>
    /// Gets the distinct group events for the specified attendees.
    /// </summary>
    /// <param name="attendees">The attendees to get the group events for.</param>
    /// <returns>The readonly collection of group events with the specified identifiers.</returns>
    Task<IReadOnlyCollection<Domain.Identity.Entities.GroupEvent>> GetForAttendeesAsync(IReadOnlyCollection<Domain.Identity.Entities.Attendee> attendees);

    /// <summary>
    /// Gets the group event with the specified name.
    /// </summary>
    /// <param name="name">The group event name.</param>
    /// <returns>The maybe instance that may contain the group event with the specified name.</returns>
    Task<Maybe<Domain.Identity.Entities.GroupEvent>> GetGroupEventByName(string name);
}