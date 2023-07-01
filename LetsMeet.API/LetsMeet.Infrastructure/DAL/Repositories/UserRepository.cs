using LetsMeet.Core.Domain.Entities;
using LetsMeet.Core.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LetsMeet.Infrastructure.DAL.Repositories;

internal sealed class UserRepository : IUserRepository
{
    private readonly DbSet<User> _users;

    public UserRepository(LetsMeetDbContext dbContext)
    {
        _users = dbContext.Users;
    }

    public Task<User> GetByIdAsync(Guid id)
        => _users.SingleOrDefaultAsync(x => x.Id == id);
    
    public Task<User> GetByEmailAsync(string email)
        => _users.SingleOrDefaultAsync(x => x.Email == email);

    public Task<User> GetByUsernameAsync(string username)
        => _users.SingleOrDefaultAsync(x => x.UserName == username);

    public async Task AddAsync(User user)
        => await _users.AddAsync(user);
}