using LetsMeet.API.Database.Entities;
using Microsoft.AspNetCore.SignalR;

namespace LetsMeet.API.Interfaces;

public interface IUserInfoProvider : IUserIdProvider
{
    bool IsLogged { get; }
    string? Id { get; }
    User CurrentUser { get; }
    string Name { get; }
}