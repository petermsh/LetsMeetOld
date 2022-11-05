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
    //profilePhoto
    public bool Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> user)
    {
        //
        // user.Property(x => x.Nick)
        //     .IsRequired();
        //
        // user.Property(x => x.Password)
        //     .IsRequired();

        user.Property(x => x.Status)
            .IsRequired()
            .HasDefaultValue(true);

        // user.HasMany(x => x.UserConnections)
        //     .WithOne(x => x.User)
        //     .HasForeignKey(x => x.UserId)
        //     .OnDelete(DeleteBehavior.ClientCascade);
        
        //user.Property(x => x.City).IsRequired();

        //user.Property(x => x.University).IsRequired();
    }
}