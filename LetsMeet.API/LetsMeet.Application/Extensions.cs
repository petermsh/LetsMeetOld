using LetsMeet.Application.Abstractions;
using LetsMeet.Application.Security;
using Microsoft.Extensions.DependencyInjection;

namespace LetsMeet.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var applicationAssembly = typeof(ICommandHandler<,>).Assembly;
        
        services.Scan(s => s.FromAssemblies(applicationAssembly)
            .AddClasses(x => x.AssignableTo(typeof(ICommandHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        services.Scan(s => s.FromAssemblies(applicationAssembly)
            .AddClasses(x => x.AssignableTo(typeof(ICommandHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}