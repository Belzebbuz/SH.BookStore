using Ardalis.Specification;

using Mapster;

using SH.Bookstore.Books.Application.Tags.DTOs;
using SH.Bookstore.Books.Application.Tags.Queries;
using SH.Bookstore.Books.Domain.Aggregates.TagAggregate;

namespace SH.Bookstore.Books.Application.Specifications.TagsSpecifications;

public class SearchTagsByFilterSpec : Specification<Tag, TagDto>
{
    internal SearchTagsByFilterSpec(SearchTagQuery filter, int page, int itemsPerPage)
    {
        Query
            .OrderBy(x => x.Title)
            .AsNoTracking();
        if (!string.IsNullOrEmpty(filter.Title))
            Query.Search(x => x.Title, $"%{filter.Title}%");

        Query
            .Skip((page - 1) * itemsPerPage)
            .Take(itemsPerPage);
        Query.Select(x => x.Adapt<TagDto>());
    }

    internal SearchTagsByFilterSpec(SearchTagQuery filter)
    {
        Query
            .OrderBy(x => x.Title)
            .AsNoTracking();
        if (!string.IsNullOrEmpty(filter.Title))
            Query.Search(x => x.Title, $"%{filter.Title}%");

        Query.Select(x => x.Adapt<TagDto>());
    }
}
