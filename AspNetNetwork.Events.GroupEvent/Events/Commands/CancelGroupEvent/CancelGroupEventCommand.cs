using AspNetNetwork.Application.Core.Abstractions.Messaging;
using AspNetNetwork.Domain.Common.Core.Primitives.Result;

namespace AspNetNetwork.Events.GroupEvent.Events.Commands.CancelGroupEvent;

/// <summary>
/// Represents the cancel group event command.
/// </summary>
public sealed record CancelGroupEventCommand(
        Guid GroupEventId,
        Guid UserId) : ICommand<Result>;