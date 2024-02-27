using System.Diagnostics;
using MediatR;
using AspNetNetwork.Application.Core.Abstractions.Messaging;
using AspNetNetwork.Application.Core.Helpers.Metric;

namespace AspNetNetwork.Application.Core.Behaviours;

/// <summary>
/// The metrics behaviour class.
/// </summary>
/// <typeparam name="TRequest">The generic request type.</typeparam>
/// <typeparam name="TResponse">The generic response type.</typeparam>
public sealed class MetricsBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest
    where TResponse : class
{
    private readonly CreateMetricsHelper _createMetricsHelper;

    /// <summary>
    /// Initializes a new instance of the <see cref="MetricsBehaviour{TRequest,TResponse}"/>; class.
    /// </summary>
    /// <param name="createMetricsHelper">The metrics helper for create metrics.</param>
    public MetricsBehaviour(CreateMetricsHelper createMetricsHelper) =>
        _createMetricsHelper = createMetricsHelper;
    
    /// <inheritdoc/>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        try
        {
            TResponse response = await next();
            stopwatch.Stop();

           await _createMetricsHelper.CreateMetrics(stopwatch);

           return response;
        }
        catch (Exception e)
        {
            throw;
        }
    }
}