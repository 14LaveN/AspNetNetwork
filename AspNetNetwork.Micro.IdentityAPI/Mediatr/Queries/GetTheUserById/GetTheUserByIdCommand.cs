namespace AspNetNetwork.Micro.IdentityAPI.Mediatr.Queries.GetTheUserById;

/// <summary>
/// Represents the get user by id command record.
/// </summary>
/// <param name="UserId">The user identifier.</param>
public sealed record GetTheUserByIdCommand(Guid UserId);