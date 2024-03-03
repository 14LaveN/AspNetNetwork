using AspNetNetwork.Domain.Identity.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Event = AspNetNetwork.Domain.Identity.Entities.Event;
using User = AspNetNetwork.Domain.Identity.Entities.User;

namespace AspNetNetwork.Database.Common.Configurations;

/// <summary>
/// Represents the configuration for the <see cref="Notification"/> entity.
/// </summary>
internal sealed class NotificationConfiguration : IEntityTypeConfiguration<Domain.Identity.Entities.Notification>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Domain.Identity.Entities.Notification> builder)
    {
        builder.HasKey(notification => notification.Id);

        builder.HasOne<Event>()
            .WithMany()
            .HasForeignKey(notification => notification.EventId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(notification => notification.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(notification => notification.NotificationType)
            .HasConversion(p => p.Value, v => NotificationType.FromValue(v))
            .IsRequired();

        builder
            .Property(notification => notification.Sent)
            .IsRequired()
            .HasDefaultValue(false);

        builder
            .Property(notification => notification.DateTimeUtc)
            .IsRequired();

        builder
            .Property(invitation => invitation.CreatedOnUtc)
            .IsRequired();

        builder
            .Property(invitation => invitation.ModifiedOnUtc);

        builder
            .Property(invitation => invitation.DeletedOnUtc);

        builder
            .Property(invitation => invitation.Deleted)
            .HasDefaultValue(false);

        builder
            .HasQueryFilter(invitation => !invitation.Deleted);
    }
}