using AspNetNetwork.Database.Common;
using AspNetNetwork.Database.Message.Data.Interfaces;
using AspNetNetwork.Domain.Common.Core.Primitives.Result;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace AspNetNetwork.Database.Message.Data.Repositories;

/// <summary>
/// Represents the messages repository class.
/// </summary>
public sealed class MessagesRepository
    :  GenericRepository<Domain.Identity.Entities.Message>, IMessagesRepository
{
    /// <summary>
    /// Initialize new instance of <see cref="MessagesRepository"/> class.
    /// </summary>
    /// <param name="dbContext"></param>
    public MessagesRepository(BaseDbContext<Domain.Identity.Entities.Message> dbContext) : base(dbContext)
    {
    }

    /// <inheritdoc />
    public new async Task Remove(Domain.Identity.Entities.Message message)
    {
        await DbContext.Set<Domain.Identity.Entities.Message>()
            .Where(x=>x.Id == message.Id)
            .ExecuteDeleteAsync();
    }

    /// <inheritdoc />
    public async Task<Result<Domain.Identity.Entities.Message>> UpdateTask(Domain.Identity.Entities.Message message)
    {
        const string sql = """
                                           UPDATE dbo.messages
                                           SET ModifiedOnUtc= @ModifiedOnUtc,
                                               Description = @Description
                                           WHERE Id = @Id AND Deleted = 0
                           """;
        
        SqlParameter[] parameters =
        {
            new("@ModifiedOnUtc", DateTime.UtcNow),
            new("@Id", message.Id),
            new("@Description", message.Description)
        };
        
        int result = await DbContext.ExecuteSqlAsync(sql, parameters);
        
        return result is not 0 ? message : throw new ArgumentException();
    }
}