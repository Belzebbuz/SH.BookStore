using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using SH.Bookstore.Identity.Infrastructure.Context.Settings;
using SH.Bookstore.Identity.Infrastructure.Identity.Entities;

namespace SH.Bookstore.Identity.Infrastructure.Context;
public class AuthorizationDbContext : IdentityDbContext<AppUser>
{
    private readonly DatabaseSettings _dbSettings;
    public AuthorizationDbContext(DbContextOptions<AuthorizationDbContext> options, IOptions<DatabaseSettings> dbOptions)
        : base(options)
    {
        _dbSettings = dbOptions.Value;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseDatabase(_dbSettings.Provider!, _dbSettings.ConnectionString!);
    }
}
