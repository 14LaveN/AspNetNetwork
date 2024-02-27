using System.Globalization;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Prometheus;
using Quartz;
using Quartz.Util;
using AspNetNetwork.Application.Core.Settings;
using AspNetNetwork.Cache.Service;
using AspNetNetwork.Domain.Common.Core.Exceptions;
using AspNetNetwork.Domain.Common.Entities;

namespace AspNetNetwork.BackgroundTasks.QuartZ.Jobs;

/// <summary>
/// Represents the save metrics job class.
/// </summary>
public sealed class SaveMetricsJob
    : IJob
{
    private readonly IMongoCollection<MetricEntity> _metricsCollection;
    private readonly IDistributedCache _distributedCache;

    /// <summary>
    /// Initialize a new instance of the <see cref="SaveMetricsJob"/>
    /// </summary>
    /// <param name="dbSettings">The mongo db settings.</param>
    /// <param name="distributedCache">The distributed cache.</param>
    public SaveMetricsJob(IOptions<MongoSettings> dbSettings,
        IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
        
        var mongoClient = new MongoClient(
            dbSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            dbSettings.Value.Database);

        _metricsCollection = mongoDatabase.GetCollection<MetricEntity>(
            dbSettings.Value.MetricsCollectionName);
    }
    
    /// <inheritdoc/>
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            Counter counter = await _distributedCache
                .GetRecordAsync<Counter>("metrics_counter-key");

            string millisecondsInString =
                await _distributedCache.GetRecordAsync<string>("metrics_request_duration_seconds-key");
            
            if (counter is null )
            {
                throw new NotFoundException(nameof(Counter), "metrics_counter-key");
            }

            if (millisecondsInString.IsNullOrWhiteSpace())
            {
                throw new NotFoundException("Milliseconds", "metrics_request_duration_seconds-key");
            }
            
            var metrics = new List<MetricEntity>
            { 
                new("AspNetNetwork_request_duration_seconds",
                    millisecondsInString),
                new(counter.Name,
                    counter.Value.ToString(CultureInfo.CurrentCulture))
            };
            
            await _metricsCollection.InsertManyAsync(metrics);
        }
        catch (Exception e)
        {
            throw;
        }
    }
}