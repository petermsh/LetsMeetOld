using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetsMeet.API.Database.Entities;

public class Room : ICreatedAt
{
    [DatabaseGenerated((DatabaseGeneratedOption.Identity))]
    public string RoomId { get; set; }
    public string? RoomName { get; set; }
    public bool isLocked { get; set; }
    public ICollection<User> Users { get; set; }
    public ICollection<Message> Messages { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> room)
    {
        room.HasKey(s => s.RoomId);

        room.Property(r => r.isLocked)
            .IsRequired()
            .HasDefaultValue(false);

        room.HasMany(x => x.Messages)
            .WithOne(x => x.Room)
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.ClientCascade);

    }
}