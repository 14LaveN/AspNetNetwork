using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Event = AspNetNetwork.Domain.Identity.Entities.Event;
using User = AspNetNetwork.Domain.Identity.Entities.User;

namespace AspNetNetwork.Database.Common.Configurations;

/// <summary>
/// Represents the configuration for the <see cref="Attendee"/> entity.
/// </summary>
internal sealed class AttendeeConfiguration : IEntityTypeConfiguration<Domain.Identity.Entities.Attendee>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Domain.Identity.Entities.Attendee> builder)
    {
        builder.ToTable("attendees");
        
        builder.HasKey(attendee => attendee.Id);

        builder.HasOne<Event>()
            .WithMany()
            .HasForeignKey(attendee => attendee.EventId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.GroupEvents)
            .WithMany(x => x.Attendees);
        
        builder.HasOne(x => x.User)
            .WithMany(x => x.Attendees)
            .HasForeignKey(x=>x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(attendee => attendee.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(attendee => attendee.Processed).IsRequired();

        builder.Property(attendee => attendee.CreatedOnUtc).IsRequired();

        builder.Property(attendee => attendee.ModifiedOnUtc);

        builder.Property(attendee => attendee.DeletedOnUtc);

        builder.Property(attendee => attendee.Deleted).HasDefaultValue(false);

        builder.HasQueryFilter(attendee => !attendee.Deleted);
    }
}