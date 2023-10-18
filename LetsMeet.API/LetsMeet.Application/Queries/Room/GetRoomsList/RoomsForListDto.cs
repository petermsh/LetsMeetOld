namespace LetsMeet.Application.Queries.Room.GetRoomsList;

public class RoomsForListDto
{
    public string RoomId { get; init; }
    public List<string> Users { get; init; }
}