using FluentValidation;
using AspNetNetwork.Application.Core.Errors;
using AspNetNetwork.Application.Core.Extensions;

namespace AspNetNetwork.Events.PersonalEvent.Events.Commands.CancelPersonalEvent;

/// <summary>
/// Represents the <see cref="CancelPersonalEventCommand"/> validator.
/// </summary>
public sealed class CancelGroupEventCommandValidator : AbstractValidator<CancelPersonalEventCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CancelGroupEventCommandValidator"/> class.
    /// </summary>
    public CancelGroupEventCommandValidator() =>
        RuleFor(x => x.PersonalEventId)
            .NotEmpty()
            .WithError(ValidationErrors.CancelPersonalEvent.PersonalEventIdIsRequired);
}