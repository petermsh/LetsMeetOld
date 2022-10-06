using LetsMeet.API.Interfaces;
using LetsMeet.API.Services;

namespace LetsMeet.API.Infrastructure;

public static class InterfaceRegistry
{
    public static IServiceCollection RegisterInterfaces(this IServiceCollection services)
    {
        return services
            .AddScoped<IUserService, UserService>()
            .AddScoped<ErrorHandlingMiddleware>()
            .AddScoped<IUserInfoProvider, UserInfoProvider>()
            .AddScoped<IHashService, HashService>()
            .AddSingleton<IAuthManager, AuthManager>()
            .AddSingleton<IHttpContextAccessor, HttpContextAccessor>();;
    }
    
}