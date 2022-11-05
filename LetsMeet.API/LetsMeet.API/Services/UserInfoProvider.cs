using System.Security.Claims;
using System.Security.Cryptography;
using LetsMeet.API.Database;
using LetsMeet.API.Database.Entities;
using LetsMeet.API.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace LetsMeet.API.Services;

internal class UserInfoProvider : IUserInfoProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly DataContext _context;

    public UserInfoProvider(IHttpContextAccessor httpContextAccessor, DataContext context)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
    }
    
    public bool IsLogged => _httpContextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    public int? Id => _id is not null && int.TryParse(_id, out var parsedId) ? parsedId : null;
    public User CurrentUser => Id is not null
        ? _context.Users.Find(Id)
        : null;

    public string Name => _httpContextAccessor?.HttpContext?.User?.Identity?.Name;

    public string GetUserId(HubConnectionContext connection)
    {
        return connection.ConnectionId;
    }

    private IEnumerable<Claim> Claims => _httpContextAccessor?.HttpContext?.User?.Claims;
    private string _id => Claims.FirstOrDefault(x => x.Type == "id")?.Value;
}