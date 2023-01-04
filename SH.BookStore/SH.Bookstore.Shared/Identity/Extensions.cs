using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using SH.Bookstore.Shared.Identity.RequestCurrentUser;

namespace SH.Bookstore.Shared.Identity;
public static class Extensions
{
    public static IApplicationBuilder UseCurrentUser(this IApplicationBuilder app) =>
        app.UseMiddleware<CurrentUserMiddleware>();
    public static IServiceCollection AddCurrentUser(this IServiceCollection services) =>
        services
            .AddScoped<CurrentUserMiddleware>()
            .AddScoped<ICurrentUser, CurrentUser>()
            .AddScoped(sp => (ICurrentUserInitializer)sp.GetRequiredService<ICurrentUser>());
}
