using Ardalis.Specification;

using SH.Bookstore.Books.Domain.Aggregates.TagAggregate;

namespace SH.Bookstore.Books.Application.Specifications.TagsSpecifications;

public class GetTagByTitleSpec : Specification<Tag>
{
    internal GetTagByTitleSpec(string title)
    {
        Query
            .Where(x => x.Title == title)
            .AsNoTracking();
    }
}
