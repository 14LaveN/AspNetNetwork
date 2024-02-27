﻿using AspNetNetwork.Application.Core.Abstractions.Messaging;
using MediatR;
using AspNetNetwork.Database.Identity.Data.Interfaces;

namespace AspNetNetwork.Application.Core.Behaviours;

/// <summary>
/// Represents the transaction behaviour middleware.
/// </summary>
/// <typeparam name="TRequest">The request type.</typeparam>
/// <typeparam name="TResponse">The response type.</typeparam>
public sealed class UserTransactionBehaviour<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
    where TResponse : class
{
    private readonly IUserUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserTransactionBehaviour{TRequest,TResponse}"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    public UserTransactionBehaviour(IUserUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

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

        await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            TResponse response = await next();

            await transaction.CommitAsync(cancellationToken);

            return response;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}