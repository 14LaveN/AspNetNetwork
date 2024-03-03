using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNetNetwork.Database.Common.Configurations;

public class GroupEventConfiguration : IEntityTypeConfiguration<Domain.Identity.Entities.GroupEvent>
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
    }
}