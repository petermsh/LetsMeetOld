using LetsMeet.Application.Abstractions;
using LetsMeet.Application.Queries.Room.GetRooms;
using LetsMeet.Core.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LetsMeet.Infrastructure.DAL.Queries.Rooms.GetRoomsHandler;

internal sealed class GetRoomsHandler : IQueryHandler<GetRooms, List<RoomsDto>>
{
    private readonly LetsMeetDbContext _dbContext;
    private readonly IUserInfoProvider _userInfoProvider;
    private readonly IUserRepository _userRepository;

    public GetRoomsHandler(LetsMeetDbContext dbContext, IUserInfoProvider userInfoProvider, IUserRepository userRepository)
    {
        _dbContext = dbContext;
        _userInfoProvider = userInfoProvider;
        _userRepository = userRepository;
    }

    public async Task<List<RoomsDto>> HandleAsync(GetRooms query)
    {
        var user = await _userRepository.GetByIdAsync(query.UserId);
        
        var rooms = await _dbContext.Rooms
        .Where(r=>r.IsLocked == false)
        .Where(r=>r.Users.Any(x=>x.Id == user.Id))
        .Select(query=>new RoomsDto
        {
            RoomId = query.RoomId,
            RoomName = query.Users.Where(x=>x.UserName != user.UserName).Select(x=>x.UserName).FirstOrDefault(),
            LastMessage = query.Messages.OrderByDescending(x=>x.CreatedAt).FirstOrDefault().Content
        }).ToListAsync();

        return rooms;
    }
}