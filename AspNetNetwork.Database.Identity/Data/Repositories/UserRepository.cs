using AspNetNetwork.Database.Common;
using AspNetNetwork.Domain.Common.Core.Primitives.Maybe;
using AspNetNetwork.Domain.Common.ValueObjects;
using Microsoft.EntityFrameworkCore;
using AspNetNetwork.Domain.Entities;
using AspNetNetwork.Domain.Identity.Entities;
using IUserRepository = AspNetNetwork.Database.Identity.Data.Interfaces.IUserRepository;

namespace AspNetNetwork.Database.Identity.Data.Repositories;

public class UserRepository(BaseDbContext userDbContext)
    : IUserRepository
{
    public async Task<Maybe<User>> GetByIdAsync(Guid userId) =>
            await userDbContext.Set<User>().FirstOrDefaultAsync(x=>x.Id == userId) 
            ?? throw new ArgumentNullException();

    public async Task<Maybe<User>> GetByNameAsync(string name) =>
        await userDbContext.Set<User>().FirstOrDefaultAsync(x=>x.UserName == name) 
        ?? throw new ArgumentNullException();

    public async Task<Maybe<User>> GetByEmailAsync(EmailAddress emailAddress) =>
        await userDbContext.Set<User>().FirstOrDefaultAsync(x=>x.EmailAddress == emailAddress) 
        ?? throw new ArgumentNullException();
}