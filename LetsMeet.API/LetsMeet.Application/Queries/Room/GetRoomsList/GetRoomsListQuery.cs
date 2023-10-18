using LetsMeet.Application.Abstractions;

namespace LetsMeet.Application.Queries.Room.GetRoomsList;

public record GetRoomsListQuery(Guid UserId) : IQuery<List<RoomsForListDto>>;