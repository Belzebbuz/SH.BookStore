using Ardalis.Specification;

using SH.Bookstore.Books.Domain.Aggregates.AuthorAggregate;

namespace SH.Bookstore.Books.Application.Specifications.AuthorsSpecifications;
public class GetAuthorsByIdsSpec : Specification<Author>
{
    internal GetAuthorsByIdsSpec(IEnumerable<Guid> ids, bool noTracking)
    {
        Query.Where(x => ids.Contains(x.Id));
        if (noTracking)
            Query.AsNoTracking();
    }
}
