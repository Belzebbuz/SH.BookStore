using Ardalis.Specification;

using SH.Bookstore.Books.Domain.Aggregates.TagAggregate;

namespace SH.Bookstore.Books.Application.Specifications.TagsSpecifications;
public class GetTagsByIdsSpec : Specification<Tag>
{
    internal GetTagsByIdsSpec(IEnumerable<Guid> ids, bool noTracking) 
    {
        Query.Where(x => ids.Contains(x.Id));
        if(noTracking)
            Query.AsNoTracking();
    }
}
