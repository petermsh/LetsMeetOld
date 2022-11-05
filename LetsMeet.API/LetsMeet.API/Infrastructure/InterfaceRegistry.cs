using LetsMeet.API.Database;
using LetsMeet.API.Hubs;
using LetsMeet.API.Interfaces;
using LetsMeet.API.Services;
using Microsoft.AspNetCore.SignalR;

namespace LetsMeet.API.Infrastructure;

public static class InterfaceRegistry
{
    public static IServiceCollection RegisterInterfaces(this IServiceCollection services)
    {
        return services
            .AddScoped<IUserService, UserService>()
            .AddScoped<IChatService, ChatService>()
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped<IMessageService, MessageService>()
            .AddScoped<ErrorHandlingMiddleware>()
            .AddScoped<IUserInfoProvider, UserInfoProvider>()
            .AddSingleton<PresenceTracker>()
            .AddSingleton<IAuthManager, AuthManager>()
            .AddSingleton<ChatRegistry>()
            .AddSingleton<IUserIdProvider, UserIdProvider>()
            .AddSingleton<IHttpContextAccessor, HttpContextAccessor>();;
    }
    
}