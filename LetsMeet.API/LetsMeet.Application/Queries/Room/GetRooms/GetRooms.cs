using LetsMeet.Application.Abstractions;

namespace LetsMeet.Application.Queries.Room.GetRooms;

public record GetRooms(Guid UserId) : IQuery<List<RoomsDto>>;