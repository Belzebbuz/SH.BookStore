using Hangfire;

using SH.Bookstore.Whs.Host.Contracts.Repositories;
using SH.Bookstore.Whs.Host.Contracts.Services;
using SH.Bookstore.Whs.Host.Mongo.Entites;

using Throw;

namespace SH.Bookstore.Whs.Host.Services;
internal class SupplierOrderProcessor : ISupplierOrderProcessor
{
    private readonly IRepository<SupplierOrder> _supplierOrderRepository;
    private readonly ILogger<SupplierOrderProcessor> _logger;
    private readonly IRepository<BookRest> _bookRestRepository;

    public SupplierOrderProcessor(IRepository<SupplierOrder> supplierOrderRepository,
        ILogger<SupplierOrderProcessor> logger,
        IRepository<BookRest> bookRestRepository)
    {
        _supplierOrderRepository = supplierOrderRepository;
        _logger = logger;
        _bookRestRepository = bookRestRepository;
    }
    public Task StartAsync(string supplierOrderId)
    {
        var supplierPickingJobId = BackgroundJob.Enqueue(() => SupplierPickingAsync(supplierOrderId));
        var supplierOnTheWayToWarehouseJobId = BackgroundJob.ContinueJobWith(
            supplierPickingJobId ,
            () => SupplierOnTheWayAsync(supplierOrderId));
        BackgroundJob.ContinueJobWith(
           supplierOnTheWayToWarehouseJobId,
           () => SupplierDeliveredAsync(supplierOrderId));
        return Task.CompletedTask;
    }

    public async Task SupplierDeliveredAsync(string supplierOrderId)
    {
        var supplierOrder = await _supplierOrderRepository.GetByIdAsync(supplierOrderId);
        supplierOrder.ThrowIfNull();
        supplierOrder.SetState(SupplierOrderState.Delivered);
        foreach (var orderBook in supplierOrder.Books)
        {
            var bookRest = await _bookRestRepository.GetSingleByFilterAsync(x => x.BookId == orderBook.BookId);
            bookRest.AddFreeCount(orderBook.Count);
            await _bookRestRepository.UpdateAsync(bookRest.Id, bookRest);
        }
        
        await _supplierOrderRepository.UpdateAsync(supplierOrderId, supplierOrder);
        _logger.LogInformation($"Books delivered to the warehouse by shift order: {supplierOrder.ShiftOrderId}");
    }

    public async Task SupplierOnTheWayAsync(string supplierOrderId)
    {
        var supplierOrder = await _supplierOrderRepository.GetByIdAsync(supplierOrderId);
        supplierOrder.ThrowIfNull();
        supplierOrder.SetState(SupplierOrderState.OnTheWay);
        await _supplierOrderRepository.UpdateAsync(supplierOrderId, supplierOrder);
        _logger.LogInformation($"Supplier on the way to the warehouse by shift order: {supplierOrder.ShiftOrderId}");
        await Task.Delay(10000);
    }

    public async Task SupplierPickingAsync(string supplierOrderId)
    {
        var supplierOrder = await _supplierOrderRepository.GetByIdAsync(supplierOrderId);
        supplierOrder.ThrowIfNull();
        supplierOrder.SetState(SupplierOrderState.Picking);
        await _supplierOrderRepository.UpdateAsync(supplierOrderId, supplierOrder);
        _logger.LogInformation($"Supplier picking books by shift order: {supplierOrder.ShiftOrderId}");
        await Task.Delay(10000);
    }
}
