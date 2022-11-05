using System.Text.Json.Serialization;

namespace LetsMeet.API.DTO;

public class MessageListDTO
{
    public string From { get; set; }
    public string Body { get; set; }
    public DateTime Date { get; set; }
    public bool FromUser { get; set; }
}

public class LastMessageDto
{
    public string Body { get; set; }
    public DateTime Date { get; set; }
}

public class MessageDto
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public string SenderUsername { get; set; }
    public string SenderPhotoUrl { get; set; }
    public int RecipientId { get; set; }
    public string RecipientUsername { get; set; }
    public string RecipientPhotoUrl { get; set; }
    public string Content { get; set; }
    public DateTime? DateRead { get; set; }
    public DateTime MessageSent { get; set; }

    [JsonIgnore]
    public bool SenderDeleted { get; set; }

    [JsonIgnore]
    public bool RecipientDeleted { get; set; }
}

public class CreateMessageDto
{
    public string RecipientUsername { get; set; }
    public string Content { get; set; }
}