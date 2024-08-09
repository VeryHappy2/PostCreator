using IdentityServerApi.Host.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServerApi.Host.Data.EntityConfigurations;

public class IdentityRefreshConfiguration
    : IEntityTypeConfiguration<RefreshTokenEntity>
{
    public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
    {
        builder.ToTable(nameof(RefreshTokenEntity));

        builder
            .HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .UseHiLo();

        builder.Property(x => x.UserId)
            .IsRequired();

        builder
            .Property(builder => builder.RefreshToken)
            .IsRequired();

        builder
            .Property(x => x.UserPasswordHash)
            .IsRequired();

        builder
            .Property(x => x.RefreshTokenExpiry)
            .IsRequired();

        builder
            .HasIndex(x => x.RefreshToken)
            .IsUnique();
    }
}