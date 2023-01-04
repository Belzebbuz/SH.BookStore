using SH.Bookstore.Books.Application.Contracts.Queries;
using SH.Bookstore.Books.Application.Contracts.Repository;
using SH.Bookstore.Books.Application.Specifications.TagsSpecifications;
using SH.Bookstore.Books.Application.Tags.DTOs;
using SH.Bookstore.Books.Domain.Aggregates.TagAggregate;
using SH.Bookstore.Shared.Common.Services.Pagination;
using SH.Bookstore.Shared.Wrapper;

namespace SH.Bookstore.Books.Application.Tags.Queries;

public class SearchTagQueryHandler : IPagedQueryHandler<SearchTagQuery, TagDto>
{
    private readonly IReadRepository<Tag> _repository;
    private readonly IPaginationService _paginationService;

    public SearchTagQueryHandler(IReadRepository<Tag> repository, IPaginationService paginationService)
    {
        _repository = repository;
        _paginationService = paginationService;
    }
    public async Task<PaginatedResult<TagDto>> Handle(SearchTagQuery request, CancellationToken cancellationToken)
    {
        var tags = await _repository.ListAsync(TagsSpec.GetByFilter(request, _paginationService.Page, _paginationService.ItemsPerPage));
        var totalCount = await _repository.CountAsync(TagsSpec.GetByFilter(request));
        return PaginatedResult<TagDto>.Success(tags, totalCount, _paginationService.Page, _paginationService.ItemsPerPage);
    }
}
