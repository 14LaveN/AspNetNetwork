using Microsoft.EntityFrameworkCore;
using AspNetNetwork.Database.Common;
using AspNetNetwork.Database.Common.Abstractions;
using AspNetNetwork.Database.Common.Specifications;
using AspNetNetwork.Database.GroupEvent.Data.Interfaces;
using AspNetNetwork.Domain.Common.Core.Primitives.Maybe;
using AspNetNetwork.Domain.Entities;
using AspNetNetwork.Domain.Identity.Entities;

namespace AspNetNetwork.Database.GroupEvent.Data.Repositories;

/// <summary>
/// Represents the group event repository.
/// </summary>
internal sealed class GroupEventRepository : GenericRepository<Domain.Identity.Entities.GroupEvent>, IGroupEventRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupEventRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public GroupEventRepository(BaseDbContext dbContext)
        : base(dbContext) { }

    /// <inheritdoc/>
    public async Task<Maybe<Domain.Identity.Entities.GroupEvent>> GetGroupEventByName(string name) =>
        (await DbContext.Set<Domain.Identity.Entities.GroupEvent>().FirstOrDefaultAsync(x => x.Name == name))!;
    
    /// <inheritdoc />
    public async Task<IReadOnlyCollection<Domain.Identity.Entities.GroupEvent>> GetForAttendeesAsync(IReadOnlyCollection<Domain.Identity.Entities.Attendee> attendees) =>
        attendees.Count is not 0
            ? await DbContext.Set<Domain.Identity.Entities.GroupEvent>()
                .Where(new GroupEventForAttendeesSpecification(attendees))
                .ToArrayAsync()
            : Array.Empty<Domain.Identity.Entities.GroupEvent>();
}