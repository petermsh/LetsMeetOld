using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetsMeet.API.Database.Entities;

public class Connection
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public string RoomId { get; set; }
    public Room Room { get; set; }
}

public class ConnectionConfiguration : IEntityTypeConfiguration<Connection>
{
    public void Configure(EntityTypeBuilder<Connection> connection)
    {
        connection.HasKey(x => x.Id)
            .IsClustered(true);
        
    }
}