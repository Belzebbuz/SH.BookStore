using SH.Bookstore.Shared.Common.DI;
using SH.Bookstore.Shared.Messaging;

namespace SH.Bookstore.Whs.Host.Contracts.Services;
public interface IShiftOrderProcessor : ISingletonService
{
    public Task StartAsync(string shiftOrderId);
}

