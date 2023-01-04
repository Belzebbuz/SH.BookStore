using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SH.Bookstore.Identity.Infrastructure.Context.Settings;

using Throw;

namespace SH.Bookstore.Identity.Infrastructure.Context;
internal static class Extensions
{
    public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration config)
    {
        var dbSettings = GetDbSettings(config);
        return services
            .AddTransient<DbSeeder>()
            .Configure<DatabaseSettings>(config.GetSection(nameof(DatabaseSettings)))
            .AddDbContext<AuthorizationDbContext>(m => m.UseDatabase(dbSettings.Provider!, dbSettings.ConnectionString!));
    }

    public static async Task InitDatabaseAsync<T>(this IApplicationBuilder app) where T : DbContext
    {
        using var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetService<T>();
        await context!.Database.MigrateAsync();
        await scope.ServiceProvider.GetService<DbSeeder>()!.SeedDataAsync();
    }

    private static DatabaseSettings GetDbSettings(IConfiguration config)
    {
        var dbSettings = config.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();
        dbSettings.Throw()
            .IfNullOrEmpty(x => x.ConnectionString)
            .IfNullOrEmpty(x => x.Provider);

        return dbSettings;
    }
    internal static DbContextOptionsBuilder UseDatabase(this DbContextOptionsBuilder builder, string dbProvider, string connectionString)
    {
        switch (dbProvider)
        {
            case DbProviderKeys.PostgreSQL:
                AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                return builder.UseNpgsql(connectionString, e =>
                    e.MigrationsAssembly("SH.Bookstore.Identity.Migrators.PostgreSQL"));

            case DbProviderKeys.SQLServer:
                return builder.UseSqlServer(connectionString, e =>
                    e.MigrationsAssembly("MES.Server.Identity.Migrators.SQLServer"));

            case DbProviderKeys.Sqlite:
                return builder.UseSqlite(connectionString, e =>
                    e.MigrationsAssembly("MES.Server.Identity.Migrators.Sqlite"));

            case DbProviderKeys.InMemory:
                return builder.UseInMemoryDatabase("MemoryDatabase");
            default:
                throw new InvalidOperationException($"DB Provider {dbProvider} is not supported.");
        }
    }
}
