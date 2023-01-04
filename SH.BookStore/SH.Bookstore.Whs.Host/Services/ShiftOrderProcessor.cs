using Hangfire;

using Mapster;

using SH.Bookstore.Shared.Notifications;
using SH.Bookstore.Whs.Host.Contracts.Repositories;
using SH.Bookstore.Whs.Host.Contracts.Services;
using SH.Bookstore.Whs.Host.Mongo.Entites;

using Throw;

namespace SH.Bookstore.Whs.Host.Services;
internal class ShiftOrderProcessor : IShiftOrderProcessor
{
    private readonly IRepository<BookRest> _bookRestRepository;
    private readonly IRepository<ShiftOrder> _shiftOrderRepository;
    private readonly IRepository<SupplierOrder> _supplierOrderRepository;
    private readonly ILogger<ShiftOrderProcessor> _logger;
    private const int RequestBooksMiltiplier = 2;
    public ShiftOrderProcessor(
        IRepository<BookRest> bookRestRepository, 
        IRepository<ShiftOrder> shiftOrderRepository,
        IRepository<SupplierOrder> supplierOrderRepository,
        ILogger<ShiftOrderProcessor> logger)
    {
        _bookRestRepository = bookRestRepository;
        _shiftOrderRepository = shiftOrderRepository;
        _supplierOrderRepository = supplierOrderRepository;
        _logger = logger;
    }
    public async Task StartAsync(string shiftOrderId)
    {
        BackgroundJob.Enqueue(() => InitalProcessAsync(shiftOrderId));
    }
    public async Task InitalProcessAsync(string shiftOrderId)
    {
        var shiftOrder = await _shiftOrderRepository.GetByIdAsync(shiftOrderId);
        shiftOrder.ThrowIfNull();
        var orderedBooksIds = shiftOrder.Books.Select(b => b.BookId);
        var booksRest = await _bookRestRepository.GetByFilterAsync(x => orderedBooksIds.Contains(x.BookId));
        booksRest.ThrowIfNull().IfCountNotEquals(shiftOrder.Books.Count);

        var supplierBooks = booksRest
            .Where(x => x.FreeCount - shiftOrder.Books.First(ob => ob.BookId == x.BookId).Count < 0)
            .Select(x => OrderBook.Create(x.BookId, (shiftOrder.Books.First(ob => ob.BookId == x.BookId).Count - x.FreeCount) * RequestBooksMiltiplier))
            .ToList();

        if (supplierBooks.Any())
        {
            var supplierOrder = SupplierOrder.Create(shiftOrder.Id, supplierBooks);
            await _supplierOrderRepository.CreateAsync(supplierOrder);
            shiftOrder.SetState(ShiftOrderState.WaitingSupplierOrder);
            await _shiftOrderRepository.UpdateAsync(shiftOrder.Id, shiftOrder);
            return;
        }

        var pickingJobId = BackgroundJob.Enqueue(() => ShiftOrderPickingAsync(shiftOrder.Id));
        var onTheWayJobId = BackgroundJob.ContinueJobWith(pickingJobId, () => ShiftOrderToWayAsync(shiftOrder.Id));
        BackgroundJob.ContinueJobWith(onTheWayJobId, () => ShiftOrderDeliveredAsync(shiftOrder.Id));
    }
    public async Task ShiftOrderDeliveredAsync(string? id)
    {
        var shiftOrder = await _shiftOrderRepository.GetByIdAsync(id);
        foreach (var orderBook in shiftOrder.Books)
        {
            var bookRest = await _bookRestRepository.GetSingleByFilterAsync(x => x.BookId == orderBook.BookId);
            bookRest.SetDeliveredCount(orderBook.Count);
            await _bookRestRepository.UpdateAsync(bookRest.Id, bookRest);
        }
        shiftOrder.SetState(ShiftOrderState.Delivered);
        await _shiftOrderRepository.UpdateAsync(shiftOrder.Id, shiftOrder);
        _logger.LogInformation($"Books delivered to the client's address. Client order Id:{shiftOrder.ClientOrderId}");
        await Task.Delay(10000);
    }

    public async Task ShiftOrderToWayAsync(string? id)
    {
        var shiftOrder = await _shiftOrderRepository.GetByIdAsync(id);
        foreach (var orderBook in shiftOrder.Books)
        {
            var bookRest = await _bookRestRepository.GetSingleByFilterAsync(x => x.BookId == orderBook.BookId);
            bookRest.SetOnTheWayCount(orderBook.Count);
            await _bookRestRepository.UpdateAsync(bookRest.Id, bookRest);
        }
        shiftOrder.SetState(ShiftOrderState.OnTheWay);
        await _shiftOrderRepository.UpdateAsync(shiftOrder.Id, shiftOrder);
        _logger.LogInformation($"Books on the way to the client's address. Client order Id:{shiftOrder.ClientOrderId}");
        await Task.Delay(10000);
    }

    public async Task ShiftOrderPickingAsync(string? id)
    {
        var shiftOrder = await _shiftOrderRepository.GetByIdAsync(id);
        foreach (var orderBook in shiftOrder.Books)
        {
            var bookRest = await _bookRestRepository.GetSingleByFilterAsync(x => x.BookId == orderBook.BookId);
            bookRest.SetReserveCount(orderBook.Count);
            await _bookRestRepository.UpdateAsync(bookRest.Id, bookRest);
        }
        shiftOrder.SetState(ShiftOrderState.Picking);
        await _shiftOrderRepository.UpdateAsync(shiftOrder.Id, shiftOrder);
        _logger.LogInformation($"Picking books by client order :{shiftOrder.ClientOrderId}");
        await Task.Delay(10000);
    }
}
