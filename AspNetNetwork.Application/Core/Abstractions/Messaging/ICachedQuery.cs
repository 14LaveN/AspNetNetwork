namespace AspNetNetwork.Application.Core.Abstractions.Messaging;

/// <summary>
/// Represents the generic Cached query class. 
/// </summary>
/// <typeparam name="TResponse">The generic response type.</typeparam>
public interface ICachedQuery<out TResponse> 
    : IQuery<TResponse>, ICachedQuery;

/// <summary>
/// Represents the Cached query class.
/// </summary>
public interface ICachedQuery
{
    /// <summary>
    /// The key.
    /// </summary>
    string Key { get; }

    /// <summary>
    /// The expiration date/time.
    /// </summary>
    TimeSpan? Expiration { get; }
}