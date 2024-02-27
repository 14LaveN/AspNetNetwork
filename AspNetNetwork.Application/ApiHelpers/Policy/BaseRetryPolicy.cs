using Polly;
using Polly.Retry;
using Polly.Timeout;

namespace AspNetNetwork.Application.ApiHelpers.Policy;

/// <summary>
/// Represents the base retry polly policy class. 
/// </summary>
public static class BaseRetryPolicy
{
    /// <summary>
    /// The <see cref="ResiliencePipeline"/> policy object.
    /// </summary>
    public static readonly ResiliencePipeline Policy = new ResiliencePipelineBuilder()
        .AddRetry(new RetryStrategyOptions
        {
            ShouldHandle =
                new PredicateBuilder()
                    .Handle<Exception>(),
            MaxRetryAttempts = 3,
            Delay = TimeSpan.FromSeconds(1),
            BackoffType =
                DelayBackoffType.Constant
        })
        .AddTimeout(new TimeoutStrategyOptions
        {
            Timeout = TimeSpan.FromSeconds(10)
        })
        .Build();
}