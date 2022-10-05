using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetsMeet.API.Database.Entities;

public class User : ICreatedAt, IModifiedAt
{
    public int Id { get; set; }
    public string Nick { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string? Description { get; set; }
    public string City { get; set; }
    public string University { get; set; }
    public string? FieldOfStudies { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> user)
    {
        user.HasKey(x => x.Id);

        user.Property(x => x.Nick).IsRequired();
        
        user.Property(x => x.Email).IsRequired();
        
        user.Property(x => x.Password).IsRequired();
        
        user.Property(x => x.City).IsRequired();
        
        user.Property(x => x.University).IsRequired();
    }
}