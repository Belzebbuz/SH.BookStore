using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace SH.Bookstore.Shared.Common.Services.Pagination;
public static class Extensions
{
    public static IServiceCollection AddPaginationMiddleware(this IServiceCollection services)
        => services.AddScoped<PaginationMiddleware>();
    public static IApplicationBuilder UsePaginationMiddleware(this IApplicationBuilder app)
        => app.UseMiddleware<PaginationMiddleware>();
}
