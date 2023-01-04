using SH.Bookstore.Whs.Host.Contracts.Repositories;
using SH.Bookstore.Whs.Host.Mongo.Repositories;

namespace SH.Bookstore.Whs.Host.Mongo;
internal static class Extensions
{
    internal static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(typeof(IRepository<>), typeof(Repository<>));
        return services.Configure<MongoSettings>(configuration.GetSection(nameof(MongoSettings)));
    }
}
