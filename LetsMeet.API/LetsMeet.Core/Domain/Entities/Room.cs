using System.ComponentModel.DataAnnotations.Schema;
using LetsMeet.Core.Domain.Common;

namespace LetsMeet.Core.Domain.Entities;

public class Room : ICreatedAt
{
    [DatabaseGenerated((DatabaseGeneratedOption.Identity))]
    public string RoomId { get; set; }
    public string? RoomName { get; set; }
    public bool IsLocked { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public int EntityStatus { get; set; } = 1;
    
    public ICollection<User> Users { get; set; }
    public ICollection<Message> Messages { get; set; }

    public Room()
    {
    }

    private Room(string roomId, string? roomName, bool isLocked, DateTimeOffset createdAt, ICollection<User> users, ICollection<Message> messages)
    {
        RoomId = roomId;
        RoomName = roomName;
        IsLocked = isLocked;
        CreatedAt = createdAt;
        Users = users;
        Messages = messages;
    }
}