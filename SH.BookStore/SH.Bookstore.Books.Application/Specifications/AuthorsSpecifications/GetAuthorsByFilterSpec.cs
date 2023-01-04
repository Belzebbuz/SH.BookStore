using Ardalis.Specification;

using Mapster;

using SH.Bookstore.Books.Application.Authors.DTOs;
using SH.Bookstore.Books.Application.Authors.Queries;
using SH.Bookstore.Books.Domain.Aggregates.AuthorAggregate;

namespace SH.Bookstore.Books.Application.Specifications.AuthorsSpecifications;

public class GetAuthorsByFilterSpec : Specification<Author, AuthorDto>
{
    internal GetAuthorsByFilterSpec(SearchAuthorsQuery query, int page, int itemsPerPage)
    {
        Query
            .OrderBy(x => x.Name)
            .AsNoTracking();
        if (!string.IsNullOrEmpty(query.Name))
            Query.Search(x => x.Name, $"%{query.Name}%");

        Query
            .Skip((page - 1) * itemsPerPage)
            .Take(itemsPerPage);
        Query.Select(x => x.Adapt<AuthorDto>());
    }

    internal GetAuthorsByFilterSpec(SearchAuthorsQuery query)
    {
        Query
            .OrderBy(x => x.Name)
            .AsNoTracking();
        if (!string.IsNullOrEmpty(query.Name))
            Query.Search(x => x.Name, $"%{query.Name}%");

        Query.Select(x => x.Adapt<AuthorDto>());
    }
}
