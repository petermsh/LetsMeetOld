using LetsMeet.API.Database.Entities;

namespace LetsMeet.API.Interfaces;

public interface IUserInfoProvider
{
    bool IsLogged { get; }
    int? Id { get; }
    User CurrentUser { get; }
}