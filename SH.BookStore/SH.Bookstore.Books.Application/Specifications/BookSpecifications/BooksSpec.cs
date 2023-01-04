namespace SH.Bookstore.Books.Application.Specifications.BookSpecifications;

public static class BooksSpec
{
    public static GetBookByIdSpec GetById(Guid id, bool noTracking = false, bool ignoreFilters = false) 
        => new GetBookByIdSpec(id, noTracking, ignoreFilters);
}