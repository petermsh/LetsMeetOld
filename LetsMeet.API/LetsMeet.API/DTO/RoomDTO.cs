namespace LetsMeet.API.DTO;

public class RoomInfoDto
{
    public string RoomId { get; init; }
    public string? RoomName { get; init; }
    public string? LastMessage { get; init; }
}

public class CreatedRoomDto
{
    public List<string> Users { get; init; }
    public string roomId { get; init; }
}