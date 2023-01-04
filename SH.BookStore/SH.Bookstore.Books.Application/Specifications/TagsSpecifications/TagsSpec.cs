using Ardalis.Specification;

using SH.Bookstore.Books.Application.Tags.Queries;
using SH.Bookstore.Books.Domain.Aggregates.TagAggregate;

namespace SH.Bookstore.Books.Application.Specifications.TagsSpecifications;

public class GetTagByIdSpec : Specification<Tag>, ISingleResultSpecification<Tag>
{
    internal GetTagByIdSpec(Guid id, bool noTracking)
    {
        Query.Where(x => x.Id == id);

        if (noTracking)
            Query.AsNoTracking();
    }
}

public static class TagsSpec
{
    public static GetTagByIdSpec GetById(Guid id, bool noTracking = false)
        => new GetTagByIdSpec(id, noTracking);

    public static GetTagsByIdsSpec GetByIds(IEnumerable<Guid> ids, bool noTracking = false)
        => new GetTagsByIdsSpec(ids, noTracking);

    public static GetTagByTitleSpec GetByTitle(string title)
        => new GetTagByTitleSpec(title);

    public static SearchTagsByFilterSpec GetByFilter(SearchTagQuery searchTagQuery, int page, int itemsPerPage)
        => new SearchTagsByFilterSpec(searchTagQuery, page, itemsPerPage);

    public static SearchTagsByFilterSpec GetByFilter(SearchTagQuery searchTagQuery)
        => new SearchTagsByFilterSpec(searchTagQuery);
}