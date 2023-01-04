using SH.Bookstore.Shared.Notifications;
using SH.Bookstore.Whs.Host.Contracts.Services;
using SH.Bookstore.Whs.Host.Mongo.Entites;
using SH.Bookstore.Whs.Host.Mongo.Entites.Events;

namespace SH.Bookstore.Whs.Host.EventHandlers;
public class SupplierDeliveredOrderEventHandler :
    IEventNotificationHandler<SupplierDeliveredOrderEvent>
{
    private readonly IShiftOrderProcessor _shiftOrderProcessor;

    public SupplierDeliveredOrderEventHandler(IShiftOrderProcessor shiftOrderProcessor)
    {
        _shiftOrderProcessor = shiftOrderProcessor;
    }
    public async Task Handle(EventNotification<SupplierDeliveredOrderEvent> notification, CancellationToken cancellationToken) 
        => await _shiftOrderProcessor.StartAsync(notification.Event.Order.ShiftOrderId);
}
