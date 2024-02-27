using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AspNetNetwork.Domain.Common.Entities;

/// <summary>
/// Represents the generic mongo entity class.
/// </summary>
public abstract class BaseMongoEntity
{
    /// <summary>
    /// Gets or sets identifier.
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    /// <summary>
    /// Gets or sets date/time created at.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}