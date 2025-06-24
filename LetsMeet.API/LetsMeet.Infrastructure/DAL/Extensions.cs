using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LetsMeet.Infrastructure.DAL;

internal static class Extensions
{
    private const string SectionName = "database";

    public static IServiceCollection AddPostgres(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection(SectionName);
        services.Configure<PostgresOptions>(section);
        var options = configuration.GetOptions<PostgresOptions>(SectionName);

        services.AddDbContext<LetsMeetDbContext>(x => x.UseNpgsql(options.ConnectionString));
        // services.AddDbContext<LetsMeetDbContext>(x => x.UseSqlServer(options.ConnectionString, sqlServerOptions =>
        // {
        //     sqlServerOptions.CommandTimeout(1);
        // }));
        services.AddHostedService<DatabaseInitializer>();
        
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        return services;
    }
    
    public static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : class, new()
    {
        var options = new T();
        var section = configuration.GetSection(sectionName);
        section.Bind(options);

        return options;
    }
}