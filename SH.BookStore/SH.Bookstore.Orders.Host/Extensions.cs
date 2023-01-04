using System.Reflection;

using MediatR;

using SH.Bookstore.Orders.Host.BackgroundServices;
using SH.Bookstore.Orders.Host.Mongo;
using SH.Bookstore.Shared.Common.DI;
using SH.Bookstore.Shared.Common.Services.Pagination;
using SH.Bookstore.Shared.Hangfire;
using SH.Bookstore.Shared.Identity;
using SH.Bookstore.Shared.Messaging.Pulsar;
using SH.Bookstore.Shared.Middlewares;
using SH.Bookstore.Shared.Notifications;
using SH.Bookstore.Shared.OpenApi;
using SH.Bookstore.Shared.Security;

namespace SH.Bookstore.Orders.Host;
internal static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuth(config["JwtKey"])
            .AddPaginationMiddleware()
            .AddBackgroundJobs(config)
            .AddCurrentUser()
            .AddRequestLogging(config)
            .AddExceptionMiddleware()
            .AddNotifications()
            .AddOpenApiDocumentation(config)
            .AddMongo(config)
            .AddMediatR(Assembly.GetExecutingAssembly())
            .AddPulsarMessaging(config)
            .AddServices()
            .AddBackgroundServices()
            .AddCors(opt => opt.AddPolicy("CorsPolicy", policy => policy.AllowAnyHeader()
                                                                .AllowAnyMethod()
                                                                .AllowCredentials()));
        return services;
    }

    public static void UseInfrastructure(this IApplicationBuilder app, IConfiguration config)
    {
        app.UseExceptionMiddleware();
        app.UsePaginationMiddleware();
        app.UseRouting();
        app.UseCors("CorsPolicy");
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCurrentUser();
        app.UseRequestLogging(config);
        app.UseHangfireDashboard(config);
        app.UseOpenApiDocumentation(config);
    }

    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapControllers();
        builder.MapNotifications();
        return builder;
    }

    internal static ConfigureHostBuilder AddConfigurations(this ConfigureHostBuilder host)
    {
        host.ConfigureAppConfiguration((context, config) =>
        {
            const string configurationsDirectory = "Configurations";
            var env = context.HostingEnvironment;
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"{configurationsDirectory}/serilog.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"{configurationsDirectory}/serilog.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"{configurationsDirectory}/hangfire.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"{configurationsDirectory}/hangfire.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
        });

        return host;
    }
}
