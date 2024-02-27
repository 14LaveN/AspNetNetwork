using Microsoft.EntityFrameworkCore;
using AspNetNetwork.Database.Common;
using AspNetNetwork.Database.Common.Abstractions;
using AspNetNetwork.Database.Common.Specifications;
using AspNetNetwork.Database.PersonalEvent.Data.Interfaces;

namespace AspNetNetwork.Database.PersonalEvent.Data.Repositories;

/// <summary>
/// Represents the attendee repository.
/// </summary>
internal sealed class PersonalEventRepository : GenericRepository<Domain.Identity.Entities.PersonalEvent>, IPersonalEventRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PersonalEventRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public PersonalEventRepository(BaseDbContext<Domain.Identity.Entities.PersonalEvent> dbContext)
        : base(dbContext)
    {
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<Domain.Identity.Entities.PersonalEvent>> GetUnprocessedAsync(int take) =>
        await DbContext.Set<Domain.Identity.Entities.PersonalEvent>()
            .Where(new UnProcessedPersonalEventSpecification())
            .OrderBy(personalEvent => personalEvent.CreatedOnUtc)
            .Take(take)
            .ToArrayAsync();
}