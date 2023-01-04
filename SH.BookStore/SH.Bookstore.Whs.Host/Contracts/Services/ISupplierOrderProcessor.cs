using SH.Bookstore.Shared.Common.DI;

namespace SH.Bookstore.Whs.Host.Contracts.Services;
public interface ISupplierOrderProcessor : ISingletonService
{
    public Task StartAsync(string supplierOrderId);
}
