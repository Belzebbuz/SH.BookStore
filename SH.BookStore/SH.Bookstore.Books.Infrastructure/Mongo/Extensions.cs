using System;
using System.Runtime.CompilerServices;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SH.Bookstore.Books.Application.Contracts.Services.Mongo;
using SH.Bookstore.Books.Domain.Common.Contracts;
using SH.Bookstore.Books.Infrastructure.Mongo.Settings;

namespace SH.Bookstore.Books.Infrastructure.Mongo;
internal static class Extensions
{
    internal static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration)
    {
        return services.Configure<MongoSettings>(configuration.GetSection(nameof(MongoSettings)));
    }

    internal static IMongoRepository GetMongoRepository(this IServiceProvider serviceProvider, IMongoEntity entity)
    {
        var mongoType = entity.GetType().GetInterfaces()
               .First(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IMongoEntity<>))
               .GetGenericArguments().First();

        return (IMongoRepository)serviceProvider.GetRequiredService(typeof(IMongoRepository<,>).MakeGenericType(entity.GetType(), mongoType));
    }
}
