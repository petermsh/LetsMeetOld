using LetsMeet.Application.Abstractions;
using LetsMeet.Application.Security;
using LetsMeet.Core.Domain.Entities;
using LetsMeet.Core.Domain.Repositories;
using LetsMeet.Infrastructure.DAL;
using LetsMeet.Infrastructure.DAL.Repositories;
using LetsMeet.Infrastructure.Services;
using LetsMeet.Infrastructure.Services.Auth;
using LetsMeet.Infrastructure.Services.EmailSender;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LetsMeet.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var infrastructureAssembly = typeof(AppOptions).Assembly;

        services.Scan(s => s.FromAssemblies(infrastructureAssembly)
            .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.AddIdentity<User, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<LetsMeetDbContext>()
            .AddDefaultTokenProviders();

        var authOptions = configuration.GetSection("Auth").Get<AuthOptions>();
        services.AddScoped<AuthOptions>(x => authOptions);

        var emailConfiguration = configuration.GetSection("EmailConfiguration")
            .Get<EmailConfiguration>();
        services.AddSingleton(emailConfiguration);
        
        services.AddAuth(authOptions);
        services.AddSwagger();
        
        services.AddScoped<UserManager<User>, UserManager<User>>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IUserInfoProvider, UserInfoProvider>();
        services.AddScoped<IAuthManager, AuthManager>();
        services.AddScoped<IEmailSender, EmailSender>();
        services.AddPostgres(configuration);
        return services;
    }
}