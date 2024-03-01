using AspNetNetwork.Domain.Identity.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNetNetwork.Database.Message.Data;

/// <summary>
/// Represents the configuration for the <see cref="Message"/> entity.
/// </summary>
public sealed class MessageConfiguration: IEntityTypeConfiguration<Domain.Identity.Entities.Message>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Domain.Identity.Entities.Message> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Description)
            .IsRequired();

        builder.HasIndex(x => x.Id)
            .HasDatabaseName("IdMessageIndex");
        
        builder.HasIndex(x => x.AuthorId)
            .HasDatabaseName("AuthorIdMessageIndex");
        
        builder.HasIndex(x => x.RecipientId)
            .HasDatabaseName("RecipientIdMessageIndex");

        builder.HasOne(x => x.Author)
            .WithMany(x => x.AuthorMessages)
            .HasForeignKey(x => x.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(x => x.Recipient)
            .WithMany(x => x.RecipientMessages)
            .HasForeignKey(x => x.RecipientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.AuthorId)
            .IsRequired();
        
        builder.Property(x => x.RecipientId)
            .IsRequired();
        
        builder.Property(x => x.IsAnswered)
            .HasDefaultValue(false);                                                                                                        
        
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