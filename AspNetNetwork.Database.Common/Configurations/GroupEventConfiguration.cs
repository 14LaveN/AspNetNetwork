using AspNetNetwork.Domain.Common.ValueObjects;
using AspNetNetwork.Domain.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNetNetwork.Database.Common.Configurations;

internal class GroupEventConfiguration : IEntityTypeConfiguration<Domain.Identity.Entities.GroupEvent>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Domain.Identity.Entities.GroupEvent> builder)
    {
        builder.ToTable("groupEvents");
        
        builder.HasMany(x => x.Attendees)
            .WithMany(x => x.GroupEvents);
        
        builder.HasOne(x => x.Author)
            .WithMany(x => x.YourGroupEvents)
            .HasForeignKey(x=>x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.OwnsOne(user => user.Name, firstNameBuilder =>
        {
            firstNameBuilder.WithOwner();

            firstNameBuilder.Property(firstName => firstName.Value)
                .HasColumnName(nameof(GroupEvent.Name))
                .HasMaxLength(Name.MaxLength)
                .IsRequired();
        });
    }
}