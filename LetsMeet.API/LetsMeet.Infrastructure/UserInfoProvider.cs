using System.Security.Claims;
using LetsMeet.Application;
using LetsMeet.Application.Abstractions;
using LetsMeet.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace LetsMeet.Infrastructure;

public class UserInfoProvider : IUserInfoProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserInfoProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string UserId => Claims.FirstOrDefault(x => x.Type == "id")?.Value;
    public string UserName => _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
    private IEnumerable<Claim> Claims => _httpContextAccessor?.HttpContext?.User?.Claims;
}