using AspNetNetwork.Application.Core.Settings;
using AspNetNetwork.Database.MetricsAndMessages.Data.Interfaces;
using AspNetNetwork.Domain.Common.Entities;
using AspNetNetwork.Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AspNetNetwork.Database.MetricsAndMessages.Data.Repositories;

/// <summary>
/// Represents the generic metrics repository class.
/// </summary>
public sealed class RabbitMessagesRepository
    : IMongoRepository<RabbitMessage>
{
    private readonly IMongoCollection<RabbitMessage> _rabbitMessagesCollection;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dbSettings"></param>
    public RabbitMessagesRepository(
        IOptions<MongoSettings> dbSettings)
    {
        var mongoClient = new MongoClient(
            dbSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            dbSettings.Value.Database);
        
        _rabbitMessagesCollection = mongoDatabase.GetCollection<RabbitMessage>(
            dbSettings.Value.MetricsCollectionName);
    }

    public async Task<List<RabbitMessage>> GetAllAsync() =>
        await _rabbitMessagesCollection.Find(_ => true).ToListAsync();

    public async Task InsertAsync(RabbitMessage type) =>
        await _rabbitMessagesCollection.InsertOneAsync(type);

    public async Task InsertRangeAsync(IEnumerable<RabbitMessage> types) =>
        await _rabbitMessagesCollection.InsertManyAsync(types);

    public async Task RemoveAsync(string id) =>
        await _rabbitMessagesCollection.DeleteOneAsync(x => x.Id == id);
}