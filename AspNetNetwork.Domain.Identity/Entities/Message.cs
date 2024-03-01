using AspNetNetwork.Domain.Common.Core.Abstractions;
using AspNetNetwork.Domain.Common.Core.Primitives;
using AspNetNetwork.Domain.Core.Utility;

namespace AspNetNetwork.Domain.Identity.Entities;

/// <summary>
/// Represents the message class.
/// </summary>
public sealed class Message
    : AggregateRoot, IAuditableEntity, ISoftDeletableEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Message"/> class.
    /// </summary>
    /// <param name="description">The description.</param>
    /// <param name="recipientId">The recipient identifier.</param>
    /// <param name="authorId">The author identifier.</param>
    public Message(string description, Guid recipientId, Guid authorId)
        : base(Guid.NewGuid())
    {
        Ensure.NotEmpty(recipientId, "The recipient identifier is required.", nameof(recipientId));
        Ensure.NotEmpty(authorId, "The author identifier is required.", nameof(authorId));
        Ensure.NotNull(description, "The description is required.", nameof(description));
        
        Description = description;
        RecipientId = recipientId;
        AuthorId = authorId;
    }

    /// <summary>
    /// Gets or sets is answered flag.
    /// </summary>
    public bool IsAnswered { get; set; }

    /// <summary>
    /// Gets or sets author.
    /// </summary>
    public User? Author { get; set; }

    /// <summary>
    /// Gets or sets author identifier.
    /// </summary>
    public Guid AuthorId { get; set; }

    /// <summary>
    /// Gets or sets recipient.
    /// </summary>
    public User? Recipient { get; set; }
    
    /// <summary>
    /// Gets or sets recipient identifier.
    /// </summary>
    public Guid RecipientId { get; set; }

    /// <summary>
    /// Gets or sets description.
    /// </summary>
    public string Description { get; set; }

    /// <inheritdoc />
    public DateTime CreatedOnUtc { get; set; }

    /// <inheritdoc />
    public DateTime? ModifiedOnUtc { get; }
    
    /// <inheritdoc />
    public DateTime? DeletedOnUtc { get; }
    
    /// <inheritdoc />
    public bool Deleted { get; }
}