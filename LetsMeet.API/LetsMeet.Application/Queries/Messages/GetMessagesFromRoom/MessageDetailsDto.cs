namespace LetsMeet.Application.Queries.Messages.GetMessagesFromRoom;

public class MessageDetailsDto
{
    public int Id { get; init; }
    public string From { get; init; }
    public string Content { get; init; }
    public DateTimeOffset Date { get; init; } 
    public bool IsFromUser { get; init; }
}