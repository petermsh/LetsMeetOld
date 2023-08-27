namespace LetsMeet.Application.DTO.Message;

public class UpdateMessageDto
{
    public UpdateMessageDto()
    {
        
    }
    
    public UpdateMessageDto(int messageId, string roomId, string content)
    {
        MessageId = messageId;
        RoomId = roomId;
        Content = content;
    }

    public string RoomId { get; private set; }
    public string Content { get; private set; }
    public int MessageId { get; private set; }
}