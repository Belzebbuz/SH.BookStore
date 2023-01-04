using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using SH.Bookstore.Identity.Infrastructure.Context;
using SH.Bookstore.Identity.Infrastructure.Identity.Entities;

namespace SH.Bookstore.Identity.Infrastructure.Identity;
internal static class Extensions
{
    public static IServiceCollection AddAuthIdentity(this IServiceCollection services) =>
       services
           .AddIdentity<AppUser, IdentityRole>(options =>
           {
               options.Password.RequiredLength = 6;
               options.Password.RequireDigit = false;
               options.Password.RequireLowercase = false;
               options.Password.RequireNonAlphanumeric = false;
               options.Password.RequireUppercase = false;
               options.User.RequireUniqueEmail = true;
           })
           .AddEntityFrameworkStores<AuthorizationDbContext>()
           .AddDefaultTokenProviders()
           .Services;
}
