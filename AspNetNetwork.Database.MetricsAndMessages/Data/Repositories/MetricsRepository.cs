using AspNetNetwork.Application.Core.Settings;
using AspNetNetwork.Database.MetricsAndMessages.Data.Interfaces;
using AspNetNetwork.Domain.Common.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Prometheus;

namespace AspNetNetwork.Database.MetricsAndMessages.Data.Repositories;

/// <summary>
/// Represents the generic metrics repository class.
/// </summary>
public sealed class MetricsRepository
    : IMongoRepository<MetricEntity>
{
    private readonly IMongoCollection<MetricEntity> _metricsCollection;

    public MetricsRepository(
        IOptions<MongoSettings> dbSettings)
    {
        var mongoClient = new MongoClient(
            dbSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            dbSettings.Value.Database);
        
        _metricsCollection = mongoDatabase.GetCollection<MetricEntity>(
            dbSettings.Value.MetricsCollectionName);
    }

    public async Task<List<MetricEntity>> GetAllAsync() =>
        await _metricsCollection.Find(_ => true).ToListAsync();

    public async Task InsertAsync(MetricEntity type) =>
        await _metricsCollection.InsertOneAsync(type);

    public async Task InsertRangeAsync(IEnumerable<MetricEntity> types) =>
        await _metricsCollection.InsertManyAsync(types);

    public async Task RemoveAsync(string id) =>
        await _metricsCollection.DeleteOneAsync(x => x.Id == id);
}