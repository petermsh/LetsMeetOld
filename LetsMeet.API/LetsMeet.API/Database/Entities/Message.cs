using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetsMeet.API.Database.Entities;

public class Message : ICreatedAt
{
    public int Id { get; set; }
    public string SenderUserName { get; set; }
    public string Content { get; set; }
    public DateTime MessageSent { get; set; } = DateTime.UtcNow;
    public string RoomId { get; set; }
    public Room Room { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class MessagenConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> message)
    {
        message.HasKey(x => x.Id)
            .IsClustered(true);

    }
}