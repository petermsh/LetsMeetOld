using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetsMeet.API.Database.Entities;

public class Room : ICreatedAt
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string RoomId { get; set; }
    public ICollection<UserConnection> UserConnections { get; set; }
    public ICollection<Message> Messages { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> room)
    {
        room.HasKey(s => s.RoomId);
    }
}