using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SH.Bookstore.Books.Infrastructure.Mapping;
using SH.Bookstore.Books.Infrastructure.Mongo;
using SH.Bookstore.Books.Infrastructure.Persistance;
using SH.Bookstore.Shared.Common.DI;
using SH.Bookstore.Shared.Common.Services.Pagination;
using SH.Bookstore.Shared.Hangfire;
using SH.Bookstore.Shared.Identity;
using SH.Bookstore.Shared.Messaging.Pulsar;
using SH.Bookstore.Shared.Middlewares;
using SH.Bookstore.Shared.Notifications;
using SH.Bookstore.Shared.OpenApi;
using SH.Bookstore.Shared.Security;

using System;
using System.Reflection;

namespace SH.Bookstore.Books.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        MapsterSettings.Configure();
        return services
            .AddAuth(config["JwtKey"])
            .AddPaginationMiddleware()
            .AddBackgroundJobs(config)
            .AddCurrentUser()
            .AddPersistance(config)
            .AddRequestLogging(config)
            .AddExceptionMiddleware()
            .AddNotifications()
            .AddMediatR(Assembly.GetExecutingAssembly())
            .AddOpenApiDocumentation(config)
            .AddMongo(config)
            .AddPulsarMessaging(config)
            .AddServices()
            .AddCors(opt => opt.AddPolicy("CorsPolicy", policy => policy.AllowAnyHeader()
                                                                .AllowAnyMethod()
                                                                .AllowCredentials()));
    }

    /// <summary>
    /// Регистрация middleware, применение миграций 
    /// </summary>
    /// <param name="app"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static async Task UseInfrastructureAsync(this IApplicationBuilder app, IConfiguration config)
    {
        await app.InitDatabaseAsync<BooksDbContext>();
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
}
