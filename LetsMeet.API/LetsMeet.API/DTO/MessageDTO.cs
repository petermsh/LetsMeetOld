using System.Text.Json.Serialization;

namespace LetsMeet.API.DTO;

public class SingleMessageAddDto
{
    public string From { get; set; }
    public string Content { get; set; }
    public string Date { get; set; }
    public string RoomId { get; set; }
}

public class SingleMessageToListDto
{
    public string From { get; set; }
    public string Content { get; set; }
    public string Date { get; set; }
    public bool FromUser { get; set; }
}

public class LastMessageDto
{
    public string Content { get; set; }
    public DateTime Date { get; set; }
}

public class CreateMessageDto
{
    public CreateMessageDto()
    {
    }

    public CreateMessageDto(string content, string room)
    {
        Content = content;
        Room = room;
    }
    public string Content { get; set; }
    public string Room { get; set; }
}