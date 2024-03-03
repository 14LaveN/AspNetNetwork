namespace AspNetNetwork.Micro.MessagingAPI.Contracts.Message.Create;

/// <summary>
/// Represents the create <see cref="Message"/> request record class.
/// </summary>
/// <param name="Description">The description.</param>
/// <param name="RecipientId">The recipient identifier.</param>
public sealed record CreateMessageRequest(
    string Description,
    Guid RecipientId);