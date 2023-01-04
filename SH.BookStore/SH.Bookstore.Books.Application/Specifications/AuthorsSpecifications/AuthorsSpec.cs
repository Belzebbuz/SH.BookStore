using SH.Bookstore.Books.Application.Authors.Queries;

namespace SH.Bookstore.Books.Application.Specifications.AuthorsSpecifications;

public static class AuthorsSpec
{
    public static GetAuthorsByIdsSpec GetByIds(IEnumerable<Guid> ids, bool noTracking = false)
        => new GetAuthorsByIdsSpec(ids, noTracking);

    public static GetAuthorsByFilterSpec GetAuthorsByFilter(SearchAuthorsQuery query, int page, int itemsPerPage)
        => new GetAuthorsByFilterSpec(query, page, itemsPerPage);
    public static GetAuthorsByFilterSpec GetAuthorsByFilter(SearchAuthorsQuery query)
        => new GetAuthorsByFilterSpec(query);
}