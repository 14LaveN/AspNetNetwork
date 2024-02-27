using FluentValidation;

namespace AspNetNetwork.Events.GroupEvent.Events.Commands.AddToGroupEventAttendee;

public sealed class AddToGroupEventAttendeeCommandValidator
    : AbstractValidator<AddToGroupEventAttendeeCommand>
{
    public AddToGroupEventAttendeeCommandValidator()
    {
        RuleFor(x =>
                x.GroupEventId).NotEqual(Guid.Empty)
            .WithMessage("You don't enter identifier.");
    }
}