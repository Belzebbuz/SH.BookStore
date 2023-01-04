using SH.Bookstore.Books.Application.Books.DTOs;
using SH.Bookstore.Books.Application.Contracts.Queries;
using SH.Bookstore.Books.Application.Contracts.Services.Mongo;
using SH.Bookstore.Books.Domain.Aggregates.BookAggrgate;
using SH.Bookstore.Books.Domain.Mongo;
using SH.Bookstore.Shared.Common.Services.Pagination;
using SH.Bookstore.Shared.Wrapper;

namespace SH.Bookstore.Books.Application.Books.Queries;
public class SearchBooksQueryHandler : IPagedQueryHandler<SearchBooksQuery, MgBook>
{
    private readonly IMongoRepository<Book, MgBook> _mongoBookRepository;
    private readonly IPaginationService _paginationService;

    public SearchBooksQueryHandler(IMongoRepository<Book, MgBook> mongoBookRepository, IPaginationService paginationService)
    {
        _mongoBookRepository = mongoBookRepository;
        _paginationService = paginationService;
    }

    public async Task<PaginatedResult<MgBook>> Handle(SearchBooksQuery request, CancellationToken cancellationToken)
        => await _mongoBookRepository.GetByFilterAsync(request, _paginationService.Page, _paginationService.ItemsPerPage);
}
