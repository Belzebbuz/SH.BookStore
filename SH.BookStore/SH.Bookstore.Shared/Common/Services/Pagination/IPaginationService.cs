using SH.Bookstore.Shared.Common.DI;

namespace SH.Bookstore.Shared.Common.Services.Pagination;

/// <summary>
/// Сервис постраничного вывода
/// </summary>
public interface IPaginationService : IScopedService
{
    /// <summary>
    /// Текущая страница
    /// </summary>
    public int Page { get; }

    /// <summary>
    /// Количество элементов на странице
    /// </summary>
    public int ItemsPerPage { get; }

    /// <summary>
    /// Установить параметры постраничного запроса
    /// </summary>
    /// <param name="page"></param>
    /// <param name="itemsPerPage"></param>
    public void SetRequestPaginate(int page, int itemsPerPage);
}
