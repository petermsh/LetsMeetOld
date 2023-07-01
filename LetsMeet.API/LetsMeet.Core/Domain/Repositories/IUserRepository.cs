using LetsMeet.Core.Domain.Entities;

namespace LetsMeet.Core.Domain.Repositories;

public interface IUserRepository
{
    Task<User> GetByIdAsync(Guid id);
    Task<User> GetByEmailAsync(string email);
    Task<User> GetByUsernameAsync(string username);
    Task AddAsync(User user);
}