using AspNetNetwork.Domain.Common.Core.Primitives.Maybe;
using AspNetNetwork.Domain.Common.Core.Primitives.Result;

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
    Task<Result<Domain.Identity.Entities.Message>> UpdateTask(Domain.Identity.Entities.Message message);

    //TODO /// <summary>
    //TODO /// Gets the enumerable messages with the specified author identifier.
    //TODO /// </summary>
    //TODO /// <param name="authorId">The author identifier.</param>
    //TODO /// <returns>The maybe instance that may contain the enumerable message DTO with the specified message class.</returns>
    //TODO Task<IEnumerable<TasksDto>> GetAuthorTasksByIsDone(Guid authorId);
    //TODO 
    //TODO /// <summary>
    //TODO /// Gets the enumerable messages with the specified company identifier.
    //TODO /// </summary>
    //TODO /// <param name="companyId">The company identifier.</param>
    //TODO /// <returns>The maybe instance that may contain the enumerable message DTO with the specified message class.</returns>
    //TODO Task<IEnumerable<Maybe<TasksDto>>> GetCompanyTasksByIsDone(Guid companyId);
}