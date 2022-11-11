using Microsoft.AspNetCore.SignalR;

namespace LetsMeet.API.Services;

public class UserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        return connection.User?.Claims.FirstOrDefault(x => x.Type == "user_id")?.Value;
    }
}