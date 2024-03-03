using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Event = AspNetNetwork.Domain.Identity.Entities.Event;
using User = AspNetNetwork.Domain.Identity.Entities.User;

namespace AspNetNetwork.Database.Common.Configurations;

/// <summary>
/// Represents the configuration for the <see cref="Invitation"/> entity.
/// </summary>
internal sealed class InvitationConfiguration : IEntityTypeConfiguration<Domain.Identity.Entities.Invitation>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Domain.Identity.Entities.Invitation> builder)
    {
        builder.ToTable("invitations");
        
        builder.HasKey(invitation => invitation.Id);

        builder.HasOne<Event>()
            .WithMany()
            .HasForeignKey(invitation => invitation.EventId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(invitation => invitation.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(invitation => invitation.Accepted).HasDefaultValue(false);

        builder.Property(invitation => invitation.Rejected).HasDefaultValue(false);

        builder.Property(invitation => invitation.CompletedOnUtc);

        builder.Property(invitation => invitation.CreatedOnUtc).IsRequired();

        builder.Property(invitation => invitation.ModifiedOnUtc);

        builder.Property(invitation => invitation.DeletedOnUtc);

        builder.Property(invitation => invitation.Deleted).HasDefaultValue(false);

        builder.HasQueryFilter(invitation => !invitation.Deleted);
    }
}