using AspNetNetwork.Domain.Common.Core.Primitives.Maybe;
using AspNetNetwork.Domain.Common.Core.Primitives.Result;
using AspNetNetwork.Domain.Message.DTO;

namespace AspNetNetwork.Database.Message.Data.Interfaces;

/// <summary>
/// Represents the messages repository interface.
/// </summary>
public interface IMessagesRepository
{
    /// <summary>
    /// Gets the message with the specified identifier.
    /// </summary>
    /// <param name="messageId">The message identifier.</param>
    /// <returns>The maybe instance that may contain the message with the specified identifier.</returns>
    Task<Maybe<Domain.Identity.Entities.Message>> GetByIdAsync(Guid messageId);

    /// <summary>
    /// Inserts the specified message to the database.
    /// </summary>
    /// <param name="message">The message to be inserted to the database.</param>
    Task Insert(Domain.Identity.Entities.Message message);

    /// <summary>
    /// Remove the specified message entity to the database.
    /// </summary>
    /// <param name="message">The message to be inserted to the database.</param>
    Task Remove(Domain.Identity.Entities.Message message);

    /// <summary>
    /// Update the specified message entity to the database.
    /// </summary>
    /// <param name="message">The message to be inserted to the database.</param>
    /// <returns>The result instance that may contain the message entity with the specified message class.</returns>
    Task<Result<Domain.Identity.Entities.Message>> UpdateMessage(Domain.Identity.Entities.Message message);

    /// <summary>
    /// Gets the enumerable messages with the specified author identifier and recipient identifier.
    /// </summary>
    /// <param name="recipientId">The recipient identifier.</param>
    /// <param name="authorId">The author identifier.</param>
    /// <param name="batchSize">The batch size.</param>
    /// <returns>The maybe instance that may contain the enumerable message DTO with the specified message class.</returns>
    Task<List<MessageDto>> GetRecipientMessagesById(Guid recipientId, Guid authorId, int batchSize);
     
    /// <summary>
    /// Gets the enumerable messages by is answered with the specified author identifier and recipient identifier.
    /// </summary>
    /// <param name="recipientId">The recipient identifier.</param>
    /// <param name="authorId">The author identifier.</param>
    /// <returns>The maybe instance that may contain the enumerable message DTO with the specified message class.</returns>
    Task<IEnumerable<MessageDto>> GetMessagesByIsAnswered(Guid recipientId, Guid authorId);
}