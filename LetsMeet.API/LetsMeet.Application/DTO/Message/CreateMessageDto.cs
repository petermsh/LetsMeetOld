namespace LetsMeet.Application.DTO.Message;

public class CreateMessageDto
{
    public CreateMessageDto()
    {
    }

    public CreateMessageDto(string room, string content)
    {
        Room = room;
        Content = content;
    }

    public string Content { get; set; }
    public string Room { get; set; }
}