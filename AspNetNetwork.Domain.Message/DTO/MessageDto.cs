namespace AspNetNetwork.Domain.Message.DTO;

/// <summary>
/// Represents  the message data transfer objects record class.
/// </summary>
/// <param name="Description">The description.</param>
/// <param name="RecipientName">The recipient name.</param>
/// <param name="AuthorName">The author name.</param>
/// <param name="CreatedOnUtc">The date/time created at.</param>
public sealed record MessageDto(string Description,
    string RecipientName,
    string AuthorName,
    DateTime CreatedOnUtc);