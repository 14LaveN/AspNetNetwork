using System.Reflection;
using AspNetNetwork.Database.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AspNetNetwork.Database.Message;

/// <summary>
/// Represents the message database context class.
/// </summary>
public sealed class MessageDbContext
    : BaseDbContext<Domain.Identity.Entities.Message>
{
    /// <summary>
    /// Gets or sets Messages.
    /// </summary>
    public DbSet<Domain.Identity.Entities.Message> Messages { get; set; } = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageDbContext"/> class.
    /// </summary>
    /// <param name="dbContextOptions">The database context options.</param>
    /// <param name="mediator"></param>
    public MessageDbContext(DbContextOptions dbContextOptions, IMediator mediator)
        : base(dbContextOptions, mediator){}

    public MessageDbContext() { }

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