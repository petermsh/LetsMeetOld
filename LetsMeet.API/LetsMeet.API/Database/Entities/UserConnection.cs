using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Win32.SafeHandles;

namespace LetsMeet.API.Database.Entities;

public class UserConnection
{
    [Key]
    public string ConnectionId { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public string RoomId { get; set; }
    public Room Rooms { get; set; }
}