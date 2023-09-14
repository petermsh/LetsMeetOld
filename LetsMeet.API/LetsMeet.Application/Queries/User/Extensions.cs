using LetsMeet.Application.Queries.User.GetUserByUserName;
using LetsMeet.Core.Domain.Enums;

namespace LetsMeet.Application.Queries.User;

public static class Extensions
{
    public static UserDetailsDto AsDto(this Core.Domain.Entities.User entity)
        => new()
        {
            Id = entity.Id,
            Bio = entity.Bio,
            City = entity.City,
            Gender = (Gender)entity.Gender,
            Major = entity.Major,
            University = entity.University,
            UserName = entity.UserName,
            Photo = Convert.FromBase64String(entity.Photo)
        };
}