using Ardalis.Specification;

using SH.Bookstore.Books.Domain.Aggregates.BookAggrgate;

namespace SH.Bookstore.Books.Application.Specifications.BookSpecifications;
public class GetBookByIdSpec : Specification<Book>, ISingleResultSpecification<Book>
{
    internal GetBookByIdSpec(Guid id, bool noTracking, bool ignoreFilters)
    {
        Query.Where(x => x.Id == id)
            .Include(x => x.Authors)
            .Include(x => x.Tags)
            .Include(x => x.Prices);

        if (noTracking)
            Query.AsNoTracking();

        if(ignoreFilters)
            Query.IgnoreQueryFilters();
    }
}
