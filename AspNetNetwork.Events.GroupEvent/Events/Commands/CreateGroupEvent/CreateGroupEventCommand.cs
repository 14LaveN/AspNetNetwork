using AspNetNetwork.Application.Core.Abstractions.Messaging;
using AspNetNetwork.Domain.Common.Core.Primitives.Result;

namespace AspNetNetwork.Events.GroupEvent.Events.Commands.CreateGroupEvent;

/// <summary>
/// Represents the create group event command.
/// </summary>
public sealed record CreateGroupEventCommand(
    Guid UserId,
    string Name, 
    int CategoryId, 
    DateTime DateTimeUtc) : ICommand<Result>;