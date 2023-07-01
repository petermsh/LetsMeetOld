using LetsMeet.Core.Domain.Common;

namespace LetsMeet.Core.Domain.Entities;

public class Message : ICreatedAt
{
    public int Id { get; set; }
    public string SenderUserName { get; set; }
    public string Content { get; set; }
    public string MessageSent { get; set; } = DateTime.UtcNow.ToString("O");
    public DateTimeOffset CreatedAt { get; set; }
    
    public string RoomId { get; set; }
    public Room Room { get; set; }
}