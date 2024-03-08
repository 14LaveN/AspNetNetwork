using AspNetNetwork.Domain.Common.Core.Errors;
using AspNetNetwork.Domain.Common.Core.Primitives.Result;
using AspNetNetwork.Domain.Common.ValueObjects;
using AspNetNetwork.Domain.Identity.Events.GroupEvent;
using AspNetNetwork.Domain.Identity.Enumerations;
using AspNetNetwork.Domain.Identity.Events.Invitation;

namespace AspNetNetwork.Domain.Identity.Entities;

/// <summary>
/// Represents a group event.
/// </summary>
public sealed class GroupEvent : Event
{
    //TODO Change all db context to one which implement IDbContext.
    
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupEvent"/> class.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="name">The event name.</param>
    /// <param name="category">The category.</param>
    /// <param name="dateTimeUtc">The date and time of the event in UTC format.</param>
    private GroupEvent(User user, Name name, Category category, DateTime dateTimeUtc)
        : base(user, name, category, dateTimeUtc, EventType.GroupEvent)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupEvent"/> class.
    /// </summary>
    /// <remarks>
    /// Required by EF Core.
    /// </remarks>
    private GroupEvent()
    {
    }

    /// <summary>
    /// Navigation field.
    /// </summary>
    public ICollection<Attendee>? Attendees { get; set; }
    
    /// <summary>
    /// Gets the user identifier.
    /// </summary>
    public override Guid UserId { get; set; }

    /// <summary>
    /// Add to group event attendee.
    /// </summary>
    /// <param name="groupEvent">The group event.</param>
    /// <param name="attendee">The attendee</param>
    /// <returns>The newly created group event.</returns>
    public static async Task<Result> AddToGroupEventAttendee(GroupEvent groupEvent, Attendee attendee)
    {
        if (!groupEvent.Cancelled)
        {
            groupEvent.AddDomainEvent(new AddToGroupEventAttendeeDomainEvent(groupEvent, attendee));

            return await Result.Success();
        }

        return Result.Failure(DomainErrors.GroupEvent.IsCancelled);
    }    

    /// <summary>
    /// Creates a new group event based on the specified parameters.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="name">The name.</param>
    /// <param name="category">The category.</param>
    /// <param name="dateTimeUtc">The date and time in UTC format.</param>
    /// <returns>The newly created group event.</returns>
    public static GroupEvent Create(User user, Name name, Category category, DateTime dateTimeUtc)
    {
        var groupEvent = new GroupEvent(user, name, category, dateTimeUtc);

        groupEvent.AddDomainEvent(new GroupEventCreatedDomainEvent(groupEvent));

        return groupEvent;
    }

    /// <summary>
    /// Gets the event owner.
    /// </summary>
    /// <returns>The event owner attendee instance.</returns>
    public Attendee GetOwner() => new Attendee(this);

    /// <inheritdoc />
    public override Result Cancel(DateTime utcNow)
    {
        Result result = base.Cancel(utcNow);

        if (result.IsSuccess)
        {
            AddDomainEvent(new GroupEventCancelledDomainEvent(this));
        }

        return result;
    }

    /// <inheritdoc />
    public override bool ChangeName(Name name)
    {
        string previousName = Name;

        bool hasChanged = base.ChangeName(name);

        if (hasChanged)
        {
            AddDomainEvent(new GroupEventNameChangedDomainEvent(this, previousName));
        }

        return hasChanged;
    }

    /// <inheritdoc />
    public override bool ChangeDateAndTime(DateTime dateTimeUtc)
    {
        DateTime previousDateAndTime = DateTimeUtc;

        bool hasChanged = base.ChangeDateAndTime(dateTimeUtc);

        if (hasChanged)
        {
            AddDomainEvent(new GroupEventDateAndTimeChangedDomainEvent(this, previousDateAndTime));
        }

        return hasChanged;
    }
}
