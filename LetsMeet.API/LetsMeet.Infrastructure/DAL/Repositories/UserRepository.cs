using System.Linq.Expressions;
using LetsMeet.Core.Domain.Entities;
using LetsMeet.Core.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LetsMeet.Infrastructure.DAL.Repositories;

internal sealed class UserRepository : IUserRepository
{
    private readonly DbSet<User> _users;
    private readonly LetsMeetDbContext _dbContext;

    public UserRepository(LetsMeetDbContext dbContext)
    {
        _dbContext = dbContext;
        _users = dbContext.Users;
    }

    public async Task<User> GetByIdAsync(Guid id)
        => await _users.SingleOrDefaultAsync(x => x.Id == id);

    public async Task<User> GetByEmailAsync(string email)
        => await _users.SingleOrDefaultAsync(x => x.Email == email);

    public async Task<User> GetByUsernameAsync(string username)
        => await _users.SingleOrDefaultAsync(x => x.UserName == username);

    public async Task AddAsync(User user)
    {
        await _users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _users.Update(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<User>> GetUsersAsync(Expression<Func<User, bool>> predicate)
        => await _dbContext.Users.Where(predicate).ToListAsync();

}