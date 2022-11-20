using System.Security.Principal;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetsMeet.API.Database.Entities;

public class User :  IdentityUser, ICreatedAt, IModifiedAt
{
    public string? Bio { get; set; }
    public string City { get; set; }
    public string? University { get; set; }
    public string? Major { get; set; }
    public bool? Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }

    public ICollection<Room>? Rooms { get; set; }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> user)
    {
        user.Property(x => x.Status)
            .IsRequired()
            .HasDefaultValue(true);

        user.Property(x => x.City).IsRequired();

        user.HasMany(x => x.Rooms)
            .WithMany(r => r.Users);
    }
}