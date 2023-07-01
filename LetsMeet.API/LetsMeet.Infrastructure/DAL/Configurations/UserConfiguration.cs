using LetsMeet.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetsMeet.Infrastructure.DAL.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.Status)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(x => x.City).IsRequired();

        builder.HasMany(x => x.Rooms)
            .WithMany(r => r.Users);
    }
}