using LetsMeet.Application.Abstractions;
using LetsMeet.Application.Queries.Room.GetRoomsList;
using LetsMeet.Core.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LetsMeet.Infrastructure.DAL.Queries.Rooms.GetRoomsListHandler;

internal sealed class GetRoomsListHandler : IQueryHandler<GetRoomsListQuery, List<RoomsForListDto>>
{
    private readonly LetsMeetDbContext _dbContext;
    private readonly IUserRepository _userRepository;

    public GetRoomsListHandler(IUserRepository userRepository, LetsMeetDbContext dbContext)
    {
        _userRepository = userRepository;
        _dbContext = dbContext;
    }

    public async Task<List<RoomsForListDto>> HandleAsync(GetRoomsListQuery query)
    {
        var user = await _userRepository.GetByIdAsync(query.UserId);
        
        var rooms = await _dbContext.Rooms
            .Where(r=>r.IsLocked == false)
            .Where(r=>r.EntityStatus == 1)
            .Where(r=>r.Users.Any(x=>x.Id == user.Id))
            .Select(q=>new RoomsForListDto
            {
                RoomId = q.RoomId,
                Users = q.Users.Select(x=>x.UserName).ToList()
            }).ToListAsync();

        return rooms;
    }
}