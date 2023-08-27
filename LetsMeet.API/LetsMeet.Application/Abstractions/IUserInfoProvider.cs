using LetsMeet.Core.Domain.Entities;

namespace LetsMeet.Application.Abstractions;

public interface IUserInfoProvider
{
    public string UserId { get; }
    public string UserName { get; }
}