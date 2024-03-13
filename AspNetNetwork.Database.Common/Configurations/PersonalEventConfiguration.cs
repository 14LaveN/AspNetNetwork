using AspNetNetwork.Domain.Common.ValueObjects;
using AspNetNetwork.Domain.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNetNetwork.Database.Common.Configurations;

/// <summary>
/// Represents the configuration for the <see cref="PersonalEvent"/> entity.
/// </summary>
internal sealed class PersonalEventConfiguration : IEntityTypeConfiguration<Domain.Identity.Entities.PersonalEvent>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Domain.Identity.Entities.PersonalEvent> builder)
    {
        builder.ToTable("personalEvents");
        
        builder.Property(personalEvent => personalEvent.Processed)
            .IsRequired()
            .HasDefaultValue(false);
        
        builder.OwnsOne(user => user.Name, firstNameBuilder =>
        {
            firstNameBuilder.WithOwner();

            firstNameBuilder.Property(firstName => firstName.Value)
                .HasColumnName(nameof(PersonalEvent.Name))
                .HasMaxLength(Name.MaxLength)
                .IsRequired();
        });
    }
}