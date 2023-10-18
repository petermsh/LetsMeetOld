using LetsMeet.Core.Domain.Common;
using LetsMeet.Core.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace LetsMeet.Core.Domain.Entities;

public sealed class User : IdentityUser<Guid>, IModifiedAt, ICreatedAt
{
    public string? Bio { get; set; }
    public string City { get; set; }
    public string? University { get; set; }
    public string? Major { get; set; }
    public bool? Status { get; set; }
    public Gender? Gender { get; set; }
    public int MessageCount { get; set; } = 0;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
    public string Photo { get; set; }

    public ICollection<Room>? Rooms { get; set; }


    public User()
    {
    }

    private User(string email, string userName, string? bio, string city, string? university, string? major, Gender? gender, string photo)
    {
        Email = email;
        UserName = userName;
        Bio = bio;
        City = city;
        University = university;
        Major = major;
        Gender = gender;
        CreatedAt = DateTimeOffset.UtcNow;
        Photo = photo;
    }

    public static User Create(string email, string userName, string? bio, string city, string? university, string? major, Gender? gender, string photo)
        => new(email, userName, bio, city, university, major, gender, photo);

    public void ChangeStatus(bool status) => Status = status;

    public void CountMessage() => MessageCount++;

}