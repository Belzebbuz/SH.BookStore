using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SH.Bookstore.Shared.Middlewares.Settings;

namespace SH.Bookstore.Shared.Middlewares;
public static class Extensions
{
    public static IServiceCollection AddExceptionMiddleware(this IServiceCollection services) =>
           services.AddScoped<ExceptionMiddleware>();

    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app) =>
        app.UseMiddleware<ExceptionMiddleware>();

    public static IServiceCollection AddRequestLogging(this IServiceCollection services, IConfiguration config)
    {
        if (GetMiddlewareSettings(config).EnableHttpLogging)
        {
            services.AddSingleton<RequestLoggingMiddleware>();
            services.AddScoped<ResponseLoggingMiddleware>();
        }

        return services;
    }

    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app, IConfiguration config)
    {
        if (GetMiddlewareSettings(config).EnableHttpLogging)
        {
            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseMiddleware<ResponseLoggingMiddleware>();
        }

        return app;
    }

    private static MiddlewareSettings GetMiddlewareSettings(IConfiguration config) =>
        config.GetSection(nameof(MiddlewareSettings)).Get<MiddlewareSettings>();
}
