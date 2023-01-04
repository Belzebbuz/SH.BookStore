using System.Reflection;

using MediatR;

using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.DependencyInjection;

namespace SH.Bookstore.Books.Application;
public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration config)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddHttpContextAccessor();
        return services;
    }
}