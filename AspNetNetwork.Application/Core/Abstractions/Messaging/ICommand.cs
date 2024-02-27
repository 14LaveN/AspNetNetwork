﻿using MediatR;

namespace AspNetNetwork.Application.Core.Abstractions.Messaging;

/// <summary>
/// Represents the command interface.
/// </summary>
/// <typeparam name="TResponse">The command response type.</typeparam>
public interface ICommand<out TResponse> : IRequest<TResponse>;