using AspNetNetwork.Domain.Common.Entities;

namespace AspNetNetwork.Database.MetricsAndMessages.Data.Interfaces;

/// <summary>
/// Represents the generic mongo repository interface.
/// </summary>
/// <typeparam name="T">The <see cref="BaseMongoEntity"/> type.</typeparam>
public interface IMongoRepository<T> 
  where T : BaseMongoEntity
{
    Task<List<T>> GetAllAsync();
  
    Task InsertAsync(T type);
    
    Task InsertRangeAsync(IEnumerable<T> types);
  
    Task RemoveAsync(string id);
}