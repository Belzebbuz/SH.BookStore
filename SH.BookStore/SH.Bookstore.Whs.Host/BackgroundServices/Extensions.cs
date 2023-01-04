namespace SH.Bookstore.Whs.Host.BackgroundServices;
internal static class Extensions
{
    internal static IServiceCollection AddBackgroundServices(this IServiceCollection services)
    {
        services.AddHostedService<ShiftOrderCreatorService>();
        services.AddHostedService<BookRestCreatingService>();
        return services;
    }
}
