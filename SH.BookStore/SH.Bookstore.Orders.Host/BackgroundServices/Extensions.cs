namespace SH.Bookstore.Orders.Host.BackgroundServices;
internal static class Extensions
{
    public static IServiceCollection AddBackgroundServices(this IServiceCollection services)
        => services.AddHostedService<ShiftOrderUpdatedService>();
}
