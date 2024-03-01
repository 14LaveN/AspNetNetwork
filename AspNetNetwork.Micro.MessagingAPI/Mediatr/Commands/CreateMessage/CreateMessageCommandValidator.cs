using AspNetNetwork.Application.Core.Errors;
using AspNetNetwork.Application.Core.Extensions;
using AspNetNetwork.Domain.Common.Core.Errors;
using FluentValidation;

namespace AspNetNetwork.Micro.MessagingAPI.Mediatr.Commands.CreateMessage;

/// <summary>
/// Represents the create message command validator class.
/// </summary>
public sealed class CreateMessageCommandValidator
     : AbstractValidator<CreateMessageCommand>
{
     /// <summary>
     /// Validate the create message command.
     /// </summary>
     public CreateMessageCommandValidator()
     {
          RuleFor(x =>
                    x.Description).NotEqual(string.Empty)
               .WithError(ValidationErrors.CreateMessage.DescriptionIsRequired)
               .MaximumLength(412)
               .WithMessage("Your description too big.");
          
          RuleFor(x =>
                    x.RecipientId).NotEqual(Guid.Empty)
               .WithError(ValidationErrors.CreateMessage.RecipientIdIsRequired);
     }
}