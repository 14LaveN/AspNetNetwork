using Microsoft.Data.SqlClient;
using AspNetNetwork.Database.Common;
using AspNetNetwork.Database.Identity;
using AspNetNetwork.Database.Notification.Data.Interfaces;
using AspNetNetwork.Domain.Identity.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNetNetwork.Database.Notification.Data.Repositories;

/// <summary>
/// Represents the notification repository.
/// </summary>
internal sealed class NotificationRepository : GenericRepository<Domain.Identity.Entities.Notification>, INotificationRepository
{
    private readonly UserDbContext _userDbContext;
    private readonly BaseDbContext<Event> _eventDbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="eventDbContext">The some event database context.</param>
    /// <param name="userDbContext">The user database context.</param>
    public NotificationRepository(
        BaseDbContext<Domain.Identity.Entities.Notification> dbContext,
        BaseDbContext<Event> eventDbContext,
        UserDbContext userDbContext)
        : base(dbContext)
    {
        _eventDbContext = eventDbContext;
        _userDbContext = userDbContext;
    }

    /// <inheritdoc />
    public async Task<(Domain.Identity.Entities.Notification Notification, Event Event, User User)[]> GetNotificationsToBeSentIncludingUserAndEvent(
        int batchSize,
        DateTime utcNow,
        int allowedNotificationTimeDiscrepancyInMinutes)
    {
        DateTime startTime = utcNow.AddMinutes(-allowedNotificationTimeDiscrepancyInMinutes);
        DateTime endTime = utcNow.AddMinutes(allowedNotificationTimeDiscrepancyInMinutes);

        var notificationsWithUsersAndEvents = await (
                from notification in DbContext.Set<Domain.Identity.Entities.Notification>()
                join @event in _eventDbContext.Set<Event>()
                    on notification.EventId equals @event.Id
                join user in _userDbContext.Set<User>()
                    on notification.UserId equals user.Id
                where !notification.Sent &&
                      notification.DateTimeUtc >= startTime &&
                      notification.DateTimeUtc <= endTime
                orderby notification.DateTimeUtc
                select new
                {
                    Notification = notification,
                    Event = @event,
                    User = user
                })
            .Take(batchSize)
            .ToArrayAsync();

        return notificationsWithUsersAndEvents.Select(x => (x.Notification, x.Event, x.User)).ToArray();
    }

    /// <inheritdoc />
    public async Task RemoveNotificationsForEventAsync(Event @event, DateTime utcNow)
    {
        const string sql = @"
                UPDATE Notification
                SET DeletedOnUtc = @DeletedOn, Deleted = @Deleted
                WHERE EventId = @EventId AND Deleted = 0";

        SqlParameter[] parameters =
        {
            new("@DeletedOn", utcNow),
            new("@Deleted", true),
            new("@EventId", @event.Id)
        };

        await DbContext.ExecuteSqlAsync(sql, parameters);
    }
}