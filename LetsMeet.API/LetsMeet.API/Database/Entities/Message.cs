namespace LetsMeet.API.Database.Entities;

public class Message
{
    public int Id { get; set; }
    public string UserNick { get; set; }
    public string Body { get; set; }
    public string RoomId { get; set; }
    public Room Room { get; set; }
    public DateTime DateTime { get; set; }
}