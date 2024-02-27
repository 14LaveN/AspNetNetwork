using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AspNetNetwork.Database.Common;

namespace AspNetNetwork.Database.Invitation;

/// <summary>
/// Represents the application database context group event class.
/// </summary>
public class InvitationDbContext
    : BaseDbContext<Domain.Identity.Entities.Invitation>
{
    /// <summary>
    /// Gets or sets Invitations.
    /// </summary>
    public DbSet<Domain.Identity.Entities.Invitation> Invitations { get; set; } = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="InvitationDbContext"/> class.
    /// </summary>
    /// <param name="dbContextOptions">The database context options.</param>
    /// <param name="mediator"></param>
    public InvitationDbContext(DbContextOptions dbContextOptions, IMediator mediator)
        : base(dbContextOptions, mediator){}
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=localhost;Port=5433;Database=PAGenericDb;User Id=postgres;Password=1111;");
    }
    
    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.HasDefaultSchema("dbo");
        
        base.OnModelCreating(modelBuilder);
    }
}