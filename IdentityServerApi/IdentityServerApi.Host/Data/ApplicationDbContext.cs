using IdentityServerApi.Host.Data.Entities;
using IdentityServerApi.Host.Data.EntityConfigurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServerApi.Host.Data;

public class ApplicationDbContext : IdentityDbContext<UserApp>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<RefreshTokenEntity> RefreshToken { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new IdentityRefreshConfiguration());
    }
}
