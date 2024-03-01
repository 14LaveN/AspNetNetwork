using AspNetNetwork.Database.Common;
using AspNetNetwork.Database.Message.Data.Interfaces;
using AspNetNetwork.Domain.Common.Core.Primitives.Result;
using AspNetNetwork.Domain.Message.DTO;
using Dapper;
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
    public async Task<Result<Domain.Identity.Entities.Message>> UpdateMessage(Domain.Identity.Entities.Message message)
    {
        const string sql = """
                                           UPDATE dbo.messages
                                           SET ModifiedOnUtc = @ModifiedOnUtc,
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

    /// <inheritdoc />
    public async Task<List<MessageDto>> GetRecipientMessagesById(Guid recipientId, Guid authorId, int batchSize)
    {
        IQueryable<Domain.Identity.Entities.Message> query = DbContext.Set<Domain.Identity.Entities.Message>()
            .AsSingleQuery()
            .AsNoTrackingWithIdentityResolution()
            .Include(x => x.Author)
            .Include(x => x.Recipient)
            .Where(x => x.RecipientId == recipientId &&
                        x.AuthorId == authorId)
            .OrderByDescending(x=>x.CreatedOnUtc);

        if (batchSize == 0)
            return await query
                .Select(x => new MessageDto(
                    x.Description,
                    x.Recipient!.UserName!,
                    x.Author!.UserName!,
                    x.CreatedOnUtc))
                .ToListAsync();
        
        return await query
            .Take(batchSize)
            .Select(x => new MessageDto(
                x.Description,
                x.Recipient!.UserName!,
                x.Author!.UserName!,
                x.CreatedOnUtc))
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<MessageDto>> GetMessagesByIsAnswered(Guid recipientId, Guid authorId)
    {
        await using var connection = new SqlConnection("");
        
        await connection.OpenAsync();
        
        var query = """
                        SELECT m.Description, m.CreatedOnUtc, u.UserName
                        FROM dbo.messages AS m
                        INNER JOIN dbo.AspNetUsers AS u
                        ON @AuthorId = u.Id
                        AND @RecipientId = u.Id
                        WHERE t.IsAnswered = true
                        GROUP BY m.Description, m.CreatedOnUtc, u.UserName
                    """;

        var tasksDto = await connection.QueryAsync<MessageDto>(query, 
            new { AuthorId = authorId, RecipientId = recipientId });
        
        return tasksDto;
    }
}