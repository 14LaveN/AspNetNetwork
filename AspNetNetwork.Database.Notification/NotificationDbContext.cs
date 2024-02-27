using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AspNetNetwork.Database.Common;

namespace AspNetNetwork.Database.Notification;

/// <summary>
/// Represents the application database context group event class.
/// </summary>
public class NotificationDbContext
    : BaseDbContext<Domain.Identity.Entities.Notification>
{
    /// <summary>
    /// Gets or sets Invitations.
    /// </summary>
    public DbSet<Domain.Identity.Entities.Notification> Notifications { get; set; } = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationDbContext"/> class.
    /// </summary>
    /// <param name="dbContextOptions">The database context options.</param>
    /// <param name="mediator"></param>
    public NotificationDbContext(DbContextOptions dbContextOptions, IMediator mediator)
        : base(dbContextOptions, mediator){}
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=localhost;Port=5433;Database=TTGenericDb;User Id=postgres;Password=1111;");
    }
    
    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.HasDefaultSchema("dbo");
        
        base.OnModelCreating(modelBuilder);
    }
}