namespace LetsMeet.Application.DTO.Message;

public class SendMessageDto
{
    public string From { get; set; }
    public string Content { get; set; }
    public string Date { get; set; }
    public string RoomId { get; set; }
}