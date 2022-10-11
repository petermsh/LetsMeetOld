namespace LetsMeet.API.DTO;

public class MessageListDTO
{
    public string From { get; set; }
    public string Body { get; set; }
    public DateTime Date { get; set; }
    public bool FromUser { get; set; }
}