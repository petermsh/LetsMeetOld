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
    public User(string email, string userName, string? bio, string city, string? university, string? major, Gender? gender)
    {
        Email = email;
        UserName = userName;
        Bio = bio;
        City = city;
        University = university;
        Major = major;
        Gender = gender;
        CreatedAt = DateTimeOffset.UtcNow;
        Photo = AddDefaultPhoto();
    }

    public static User Create(string email, string userName, string? bio, string city, string? university, string? major, Gender? gender)
        => new(email, userName, bio, city, university, major, gender);

    public void ChangeStatus(bool status) => Status = status;

    public void CountMessage() => MessageCount++;

    private static string AddDefaultPhoto()
    {
        using var stream = new FileStream("./../profile.png", FileMode.Open);
        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        return Convert.ToBase64String(memoryStream.ToArray());
    }

}