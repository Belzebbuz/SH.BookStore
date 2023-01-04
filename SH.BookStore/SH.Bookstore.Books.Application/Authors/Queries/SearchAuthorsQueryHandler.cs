using SH.Bookstore.Books.Application.Authors.DTOs;
using SH.Bookstore.Books.Application.Contracts.Queries;
using SH.Bookstore.Books.Application.Contracts.Repository;
using SH.Bookstore.Books.Application.Specifications.AuthorsSpecifications;
using SH.Bookstore.Books.Domain.Aggregates.AuthorAggregate;
using SH.Bookstore.Shared.Common.Services.Pagination;
using SH.Bookstore.Shared.Wrapper;

namespace SH.Bookstore.Books.Application.Authors.Queries;

public class SearchAuthorsQueryHandler : IPagedQueryHandler<SearchAuthorsQuery, AuthorDto>
{
    private readonly IReadRepository<Author> _repository;
    private readonly IPaginationService _paginationService;

    public SearchAuthorsQueryHandler(IReadRepository<Author> repository, IPaginationService paginationService)
    {
        _repository = repository;
        _paginationService = paginationService;
    }

    public async Task<PaginatedResult<AuthorDto>> Handle(SearchAuthorsQuery request, CancellationToken cancellationToken)
    {
        var authors = await _repository.ListAsync(AuthorsSpec.GetAuthorsByFilter(request, _paginationService.Page, _paginationService.ItemsPerPage));
        var totalCount = await _repository.CountAsync(AuthorsSpec.GetAuthorsByFilter(request));
        return PaginatedResult<AuthorDto>.Success(authors, totalCount, _paginationService.Page, _paginationService.ItemsPerPage);
    }
}
