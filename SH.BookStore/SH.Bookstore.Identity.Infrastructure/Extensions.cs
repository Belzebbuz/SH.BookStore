using System.Reflection;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SH.Bookstore.Identity.Infrastructure.Context;
using SH.Bookstore.Identity.Infrastructure.Context.Settings;
using SH.Bookstore.Identity.Infrastructure.Identity;
using SH.Bookstore.Shared.Common.DI;
using SH.Bookstore.Shared.Common.Services.Pagination;
using SH.Bookstore.Shared.Identity;
using SH.Bookstore.Shared.Middlewares;
using SH.Bookstore.Shared.Notifications;
using SH.Bookstore.Shared.OpenApi;
using SH.Bookstore.Shared.Security;

namespace SH.Bookstore.Identity.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<SecuritySettings>(config.GetSection(nameof(SecuritySettings)));
        services.Configure<ServicesSeedSettings>(config.GetSection(nameof(ServicesSeedSettings)));
        return services
            .AddAuth(config["SecuritySettings:JwtSettings:Key"])
            .AddMediatR(Assembly.GetExecutingAssembly())
            .AddServices()
            .AddOpenApiDocumentation(config)
            .AddPaginationMiddleware()
            .AddCurrentUser()
            .AddAuthIdentity()
            .AddPersistance(config)
            .AddRequestLogging(config)
            .AddExceptionMiddleware()
            .AddNotifications()
            .AddCors(opt => opt.AddPolicy("CorsPolicy", policy => policy.AllowAnyHeader()
                                                                .AllowAnyMethod()
                                                                .AllowCredentials()));
    }

    public static async Task UseInfrastructureAsync(this IApplicationBuilder app, IConfiguration config)
    {
        await app.InitDatabaseAsync<AuthorizationDbContext>();
        app.UseExceptionMiddleware();
        app.UseRouting();
        app.UseCors("CorsPolicy");
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCurrentUser();
        app.UsePaginationMiddleware();
        app.UseRequestLogging(config);
        app.UseOpenApiDocumentation(config);
    }
}
