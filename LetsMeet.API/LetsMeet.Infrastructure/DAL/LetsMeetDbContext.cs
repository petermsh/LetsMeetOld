using LetsMeet.Core.Domain.Common;
using LetsMeet.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LetsMeet.Infrastructure.DAL;

internal sealed class LetsMeetDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public LetsMeetDbContext(DbContextOptions<LetsMeetDbContext> dbContextOptions) : base(dbContextOptions)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        base.OnModelCreating(modelBuilder);
    }
    
    #region ICreatedAt SaveChanges update

    private void UpdateTimestamps()
    {
        foreach (var entity in ChangeTracker.Entries().Where(p => p.State == EntityState.Added))
            if (entity.Entity is ICreatedAt created)
                created.CreatedAt = DateTime.UtcNow;

        foreach (var entity in ChangeTracker.Entries().Where(p => p.State == EntityState.Modified))
            if (entity.Entity is IModifiedAt updated)
                updated.ModifiedAt = DateTime.UtcNow;
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        UpdateTimestamps();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = new())
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    #endregion
}