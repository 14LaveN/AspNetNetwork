using AspNetNetwork.Application.ApiHelpers.Responses;
using AspNetNetwork.Application.Core.Abstractions.Messaging;
using AspNetNetwork.Domain.Common.Core.Primitives.Result;
using AspNetNetwork.Domain.Identity.Entities;

namespace AspNetNetwork.Events.GroupEvent.Events.Commands.AddToGroupEventAttendee;

/// <summary>
/// Represents the add to group event attendee command record class.
/// </summary>
/// <param name="GroupEventId">The group event identifier.</param>
/// <param name="Attendee">The attendee.</param>
public sealed record AddToGroupEventAttendeeCommand(
    Guid GroupEventId,
    Attendee Attendee) : ICommand<IBaseResponse<Result>>;