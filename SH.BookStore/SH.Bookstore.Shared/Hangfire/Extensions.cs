using System;

using Hangfire.Console.Extensions;
using Hangfire;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Hangfire.PostgreSql;
using Hangfire.SqlServer;
using Hangfire.Console;
using HangfireBasicAuthenticationFilter;
using Microsoft.AspNetCore.Builder;
using SH.Bookstore.Shared.Hangfire;
using Throw;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Hangfire.Mongo.Migration.Strategies;
using MongoDB.Driver;
using System.Reflection;

namespace SH.Bookstore.Shared.Hangfire;
public static class Extensions
{
    public static IServiceCollection AddBackgroundJobs(this IServiceCollection services, IConfiguration config)
    {

        services.AddHangfireServer(options => config.GetSection("HangfireSettings:Server").Bind(options));

        services.AddHangfireConsoleExtensions();

        var storageSettings = config.GetSection("HangfireSettings:Storage").Get<HangfireStorageSettings>();

        storageSettings.StorageProvider.ThrowIfNull();
        storageSettings.ConnectionString.ThrowIfNull();


        services.AddHangfire((provider, hangfireConfig) => hangfireConfig
            .UseDatabase(storageSettings.StorageProvider, storageSettings.ConnectionString, config)
            .UseConsole());
        return services;
    }
    private static IGlobalConfiguration UseDatabase(this IGlobalConfiguration hangfireConfig, string dbProvider, string connectionString, IConfiguration config) =>
        dbProvider.ToLowerInvariant() switch
        {
            DbProviderKeys.Npgsql =>
                hangfireConfig.UsePostgreSqlStorage(connectionString, config.GetSection("HangfireSettings:Storage:Options").Get<PostgreSqlStorageOptions>()),
            DbProviderKeys.SqlServer =>
                hangfireConfig.UseSqlServerStorage(connectionString, config.GetSection("HangfireSettings:Storage:Options").Get<SqlServerStorageOptions>()),
            DbProviderKeys.MongoDb =>
                hangfireConfig.UseMongoStorage(connectionString, config["HangfireSettings:Storage:StorageName"], new MongoStorageOptions()
                {
                    MigrationOptions = new MongoMigrationOptions
                    {
                        MigrationStrategy = new MigrateMongoMigrationStrategy(),
                        BackupStrategy = new CollectionMongoBackupStrategy(),
                    },
                    Prefix = "hangfire.mongo",
                    CheckConnection = true,
                    CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.TailNotificationsCollection
                }),
            _ => throw new Exception($"Hangfire Storage Provider {dbProvider} is not supported.")
        };

    public static IApplicationBuilder UseHangfireDashboard(this IApplicationBuilder app, IConfiguration config)
    {
        var dashboardOptions = config.GetSection("HangfireSettings:Dashboard").Get<DashboardOptions>();

        var customAuthFilter = new HangfireCustomBasicAuthenticationFilter
        {
            User = config.GetSection("HangfireSettings:Credentials:User").Value,
            Pass = config.GetSection("HangfireSettings:Credentials:Password").Value
        };

        //dashboardOptions.Authorization = new[]
        //{
        //    customAuthFilter
        //};
        //dashboardOptions.IsReadOnlyFunc = (context) => !customAuthFilter.Authorize(context);

        return app.UseHangfireDashboard(config["HangfireSettings:Route"], dashboardOptions);
    }
}
