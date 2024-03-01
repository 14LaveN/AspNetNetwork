using AspNetNetwork.Domain.Common.Core.Errors;
using AspNetNetwork.Domain.Common.Core.Primitives.Result;
using AspNetNetwork.Domain.Message.Events;

namespace AspNetNetwork.Domain.Message.Entities;

/// <summary>
/// Represents the message extensions class. 
/// </summary>
public static class MessageExtensions
{
    /// <summary>
    /// Creates a new message with the specified description and author identifier.
    /// </summary>
    /// <param name="description">The description.</param>
    /// <param name="authorId">The author identifier.</param>
    /// <param name="recipientId">The recipient identifier.</param>
    /// <returns>The newly created user instance.</returns>
    public static Identity.Entities.Message Create(
        this Identity.Entities.Message mes,
        string description,
        Guid authorId,
        Guid recipientId)
    {
        var message = new Identity.Entities.Message(description, recipientId, authorId);
        
        message.AddDomainEvent(new MessageCreatedDomainEvent(message));

        return message;
    }
    
    /// <summary>
    /// Answered  message with the specified identifier.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <returns>The result.</returns>
    public static Result AnsweredMessage(
        this Identity.Entities.Message message)
    {
        if (message.IsAnswered)
        {
            return Result.Failure(DomainErrors.Message.AlreadyAnswered);
        }

        message.IsAnswered = true;

        return Result.Success().GetAwaiter().GetResult();
    }
}