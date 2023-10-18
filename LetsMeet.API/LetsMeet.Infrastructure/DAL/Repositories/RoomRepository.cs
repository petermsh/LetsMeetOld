using System.Linq.Expressions;
using LetsMeet.Core.Domain.Entities;
using LetsMeet.Core.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LetsMeet.Infrastructure.DAL.Repositories;

internal class RoomRepository : IRoomRepository
{
    private readonly LetsMeetDbContext _dbContext;
    private readonly DbSet<Room> _rooms;

    public RoomRepository(LetsMeetDbContext dbContext)
    {
        _dbContext = dbContext;
        _rooms = dbContext.Rooms;
    }
    
    public async Task AddAsync(Room room)
    {
        await _rooms.AddAsync(room);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(Room room)
    {
        room.EntityStatus = 0;
        _rooms.Update(room);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Room room)
    {
        _rooms.Update(room);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Room> GetAsync(string id)
    { 
        return await _rooms.SingleOrDefaultAsync(x => x.RoomId == id && x.EntityStatus == 1);
    }

    public async Task<Room> GetRoomWhereUsersAsync(Guid secondUserId, Guid currentUserId)
        => await _rooms.FirstOrDefaultAsync(r => r.Users.Any(u => u.Id == secondUserId)
                                                  && r.Users.Any(u => u.Id == currentUserId)
                                                  && r.EntityStatus == 1);

    public async Task<List<Room>> GetRoomsAsync(Expression<Func<Room, bool>> predicate)
        => await _dbContext.Rooms.Where(predicate).ToListAsync();
}