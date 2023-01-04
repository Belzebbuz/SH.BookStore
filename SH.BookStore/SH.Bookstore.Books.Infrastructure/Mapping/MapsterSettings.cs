using Mapster;

using SH.Bookstore.Books.Domain.Aggregates.BookAggrgate;
using SH.Bookstore.Books.Domain.Mongo;

namespace SH.Bookstore.Books.Infrastructure.Mapping;
internal class MapsterSettings
{
    public static void Configure()
    {
        TypeAdapterConfig.GlobalSettings.Default.MaxDepth(3);
        TypeAdapterConfig.GlobalSettings.NewConfig<Book, MgBook>()
            .Map(dest => dest.Price, source => source.Prices.OrderByDescending(x => x.ActualDate).FirstOrDefault(x => x.ActualDate <= DateTime.Now)!.Value);
    }
}
