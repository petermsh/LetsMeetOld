using Microsoft.EntityFrameworkCore;

namespace LetsMeet.API.Database;

internal class DbMigrator : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    public DbMigrator(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        await context.Database.MigrateAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) =>
        Task.CompletedTask;
}