﻿using AspNetNetwork.Application.Core.Abstractions.Messaging;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using ValidationException = AspNetNetwork.Application.Core.Exceptions.ValidationException;

namespace AspNetNetwork.Application.Core.Behaviours;

/// <summary>
/// Represents the validation behaviour middleware.
/// </summary>
/// <typeparam name="TRequest">The request type.</typeparam>
/// <typeparam name="TResponse">The response type.</typeparam>
public sealed class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
    where TResponse : class
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationBehaviour{TRequest,TResponse}"/> class.
    /// </summary>
    /// <param name="validators">The validator for the current request type.</param>
    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

    /// <inheritdoc />
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is IQuery<TResponse>)
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        List<ValidationFailure> failures = _validators
            .Select(async v => await v.ValidateAsync(context, cancellationToken))
            .SelectMany(result => result.Result.Errors)
            .Where(f => f is not null)
            .ToList();

        if (failures.Count != 0)
        {
            throw new ValidationException(failures);
        }

        return await next();
    }
}