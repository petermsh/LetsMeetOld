using System.Linq.Expressions;
using LetsMeet.Core.Domain.Entities;

namespace LetsMeet.Core.Domain.Repositories;

public interface IRoomRepository
{
    Task AddAsync(Room message);
    Task RemoveAsync(Room message);
    Task UpdateAsync(Room message);
    Task<Room> GetAsync(string id);
    Task<Room> GetRoomWhereUsersAsync(Guid secondUserId, Guid currentUserId);
    Task<List<Room>> GetRoomsAsync(Expression<Func<Room, bool>> predicate);
}