using AspNetNetwork.Domain.Common.Core.Abstractions;
using AspNetNetwork.Domain.Common.Core.Primitives;
using AspNetNetwork.Domain.Common.ValueObjects;
using AspNetNetwork.Domain.Core.Utility;
using AspNetNetwork.Domain.Identity.Entities;

namespace AspNetNetwork.Domain.Company.Entities;

/// <summary>
/// Represents the company entity class.
/// </summary>
public sealed class Company
    : AggregateRoot, ISoftDeletableEntity, IAuditableEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Company"/>class.
    /// </summary>
    /// <param name="authorId">The author identifier.</param>
    /// <param name="companyName">The company name.</param>
    /// <param name="description">The description.</param>
    /// <param name="createdAt">The created at date/time.</param>
    public Company(Guid authorId, Name companyName, string description, DateTime createdAt)
        : base(Guid.NewGuid())
    {
        Ensure.NotNull(companyName, "The company name is required.", nameof(companyName));
        Ensure.NotNull(description, "The description is required.", nameof(description));
        Ensure.NotEmpty(createdAt, "The date and time is required.", nameof(createdAt));
        Ensure.NotEmpty(authorId, "The author identifier is required.", nameof(authorId));

        AuthorId = authorId;
        CreatedAt = createdAt;
        CompanyName = companyName;
        Description = description;
    }

    public Company()
    {
    }

    /// <summary>
    /// Gets or sets name.
    /// </summary>
    public Name CompanyName { get; set; } = null!;

    /// <summary>
    /// Gets or sets date/time created at.
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// Gets or sets description.
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// Navigation field.
    /// </summary>
    public ICollection<User>? Users { get; set; }

    /// <summary>
    /// Gets or sets author.
    /// </summary>
    public User? Author { get; set; }

    /// <summary>
    /// Gets or sets author identifier.
    /// </summary>
    public Guid AuthorId { get; set; }

    /// <inheritdoc />
    public DateTime CreatedOnUtc { get; }
    
    /// <inheritdoc />
    public DateTime? ModifiedOnUtc { get; }
    
    /// <inheritdoc />
    public DateTime? DeletedOnUtc { get; }
    
    /// <inheritdoc />
    public bool Deleted { get; }
}