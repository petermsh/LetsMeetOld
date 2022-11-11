using System.Text.Json.Serialization;

namespace LetsMeet.API.DTO;

public class SingleMessageDto
{
    public string From { get; set; }
    public string Content { get; set; }
    public DateTime Date { get; set; }
}

public class LastMessageDto
{
    public string Content { get; set; }
    public DateTime Date { get; set; }
}

public class CreateMessageDto
{
    public string Content { get; set; }
    public string RoomId { get; set; }
}