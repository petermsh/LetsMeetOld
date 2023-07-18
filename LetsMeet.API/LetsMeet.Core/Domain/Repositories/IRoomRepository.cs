using LetsMeet.Core.Domain.Entities;

namespace LetsMeet.Core.Domain.Repositories;

public interface IRoomRepository
{
    Task AddAsync(Room message);
    Task RemoveAsync(Room message);
    Task UpdateAsync(Room message);
    Task GetAsync(Guid id);
}