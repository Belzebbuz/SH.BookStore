namespace SH.Bookstore.Orders.Host.Mongo;
internal static class Extensions
{
    internal static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration)
    {
        return services.Configure<MongoSettings>(configuration.GetSection(nameof(MongoSettings)));
    }
}
