using LetsMeet.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetsMeet.Infrastructure.DAL.Configurations;

internal sealed class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasKey(s => s.RoomId);

        builder.Property(r => r.IsLocked)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasMany(x => x.Messages)
            .WithOne(x => x.Room)
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}