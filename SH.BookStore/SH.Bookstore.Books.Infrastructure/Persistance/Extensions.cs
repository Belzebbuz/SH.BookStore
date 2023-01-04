using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SH.Bookstore.Books.Application.Contracts.Repository;
using SH.Bookstore.Books.Domain.Common.Contracts;
using SH.Bookstore.Books.Infrastructure.Persistance.Settings;

using Throw;

namespace SH.Bookstore.Books.Infrastructure.Persistance;
internal static class Extensions
{
    public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration config)
    {
        var dbSettings = GetDbSettings(config);
        return services
            .Configure<DatabaseSettings>(config.GetSection(nameof(DatabaseSettings)))
            .AddTransient<DbSeeder>()
            .AddDbContext<BooksDbContext>(m => m.UseDatabase(dbSettings.Provider!, dbSettings.ConnectionString!))
            .AddRepositories();
    }

    public static async Task InitDatabaseAsync<T>(this IApplicationBuilder app) where T : DbContext
    {
        using var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetService<T>();
        await context!.Database.MigrateAsync();
        await scope.ServiceProvider.GetRequiredService<DbSeeder>().SeedAsync();
    }

    private static DatabaseSettings GetDbSettings(IConfiguration config)
    {
        var dbSettings = config.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();
        dbSettings.Throw()
            .IfNullOrEmpty(x => x.ConnectionString)
            .IfNullOrEmpty(x => x.Provider);
        return dbSettings;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(BookDbRepository<>));

        foreach (var aggregateRootType in
                 typeof(IAggregateRoot).Assembly.GetExportedTypes()
                     .Where(t => typeof(IAggregateRoot).IsAssignableFrom(t) && t.IsClass)
                     .ToList())
            services.AddScoped(typeof(IReadRepository<>).MakeGenericType(aggregateRootType), sp =>
                sp.GetRequiredService(typeof(IRepository<>).MakeGenericType(aggregateRootType)));
        return services;
    }

    internal static DbContextOptionsBuilder UseDatabase(this DbContextOptionsBuilder builder, string dbProvider, string connectionString)
    {
        switch (dbProvider)
        {
            case DbProviderKeys.PostgreSQL:
                AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                return builder.UseNpgsql(connectionString, e =>
                    e.MigrationsAssembly("SH.Bookstore.Books.Migrators.PostgreSQL"));

            case DbProviderKeys.SQLServer:
                return builder.UseSqlServer(connectionString, e =>
                    e.MigrationsAssembly("MES.Server.Production.Migrators.SQLServer"));

            case DbProviderKeys.Sqlite:
                return builder.UseSqlite(connectionString, e =>
                    e.MigrationsAssembly("MES.Server.Production.Migrators.Sqlite"));

            case DbProviderKeys.InMemory:
                return builder.UseInMemoryDatabase("MemoryDatabase");
            default:
                throw new InvalidOperationException($"DB Provider {dbProvider} is not supported.");
        }
    }
}
