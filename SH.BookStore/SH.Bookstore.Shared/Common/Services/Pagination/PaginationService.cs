namespace SH.Bookstore.Shared.Common.Services.Pagination;

internal class PaginationService : IPaginationService
{
    private int _page;
    public int Page => _page;

    private int _itemsPerPage;
    public int ItemsPerPage => _itemsPerPage;

    public void SetRequestPaginate(int page, int itemsPerPage)
    {
        _page = page;
        _itemsPerPage = itemsPerPage;
    }
}