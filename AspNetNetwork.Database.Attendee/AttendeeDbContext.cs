using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AspNetNetwork.Database.Common;
using AspNetNetwork.Domain.Common.Enumerations;
using AspNetNetwork.Domain.Common.ValueObjects;
using AspNetNetwork.Domain.Identity.Entities;
using AspNetNetwork.Domain.Identity.Enumerations;

namespace AspNetNetwork.Database.Attendee;

/// <summary>
/// Represents the application database context answer class.
/// </summary>
public sealed class AttendeeDbContext
    : BaseDbContext<Domain.Identity.Entities.Attendee>
{
    /// <summary>
    /// Gets or sets group events
    /// </summary>
    public DbSet<Domain.Identity.Entities.Attendee> Attendees { get; set; } = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="AttendeeDbContext"/> class.
    /// </summary>
    /// <param name="dbContextOptions">The database context options.</param>
    /// <param name="mediator"></param>
    public AttendeeDbContext(DbContextOptions<AttendeeDbContext> dbContextOptions, IMediator mediator)
        : base(dbContextOptions, mediator){}
    
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=localhost;Port=5433;Database=PAGenericDb;User Id=postgres;Password=1111;");
    }
    
    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<Event>()
            .HasKey(a => a.Id);

        modelBuilder.Entity<Event>()
           .Ignore(a => a.Category);

        modelBuilder.Entity<Category>()
            .HasNoKey();

        modelBuilder.Entity<Event>().OwnsOne(ev => ev.Name, emailBuilder =>
        {
            emailBuilder.WithOwner();

            emailBuilder.Property(ev => ev.Value)
                .HasColumnName(nameof(Event.Name))
                .IsRequired();
        });

        modelBuilder.Entity<User>().OwnsOne(user => user.EmailAddress, emailBuilder =>
        {
            emailBuilder.WithOwner();

            emailBuilder.Property(email => email.Value)
                .HasColumnName(nameof(User.EmailAddress))
                .HasMaxLength(EmailAddress.MaxLength)
                .IsRequired();
        });

        modelBuilder.Entity<User>().OwnsOne(user => user.FirstName, firstNameBuilder =>
        {
            firstNameBuilder.WithOwner();

            firstNameBuilder.Property(firstName => firstName.Value)
                .HasColumnName(nameof(User.FirstName))
                .HasMaxLength(FirstName.MaxLength)
                .IsRequired();
        });

        modelBuilder.Entity<User>().OwnsOne(user => user.LastName, lastNameBuilder =>
        {
            lastNameBuilder.WithOwner();

            lastNameBuilder.Property(lastName => lastName.Value)
                .HasColumnName(nameof(User.LastName))
                .HasMaxLength(LastName.MaxLength)
                .IsRequired();
        });

        modelBuilder.HasDefaultSchema("dbo");
        
        base.OnModelCreating(modelBuilder);
    }
}