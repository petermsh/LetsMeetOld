using LetsMeet.API.Database;
using LetsMeet.API.Database.Entities;
using LetsMeet.API.DTO;
using LetsMeet.API.Hubs;
using LetsMeet.API.Interfaces;
using LetsMeet.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace LetsMeet.API.Infrastructure;

public static class InterfaceRegistry
{
    public static IServiceCollection RegisterInterfaces(this IServiceCollection services)
    {
        return services
            .AddScoped<IEmailSender, EmailSender>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IChatService, ChatService>()
            .AddScoped<IRoomService, RoomService>()     
            .AddScoped<IMessageService, MessageService>()
            .AddScoped<ErrorHandlingMiddleware>()
            .AddScoped<UserManager<User>, UserManager<User>>()
            .AddScoped<IUserInfoProvider, UserInfoProvider>()
            .AddSingleton<IAuthManager, AuthManager>()
            .AddSingleton<IUserIdProvider, UserIdProvider>()
            .AddSingleton<IHttpContextAccessor, HttpContextAccessor>();;
    }
    
}