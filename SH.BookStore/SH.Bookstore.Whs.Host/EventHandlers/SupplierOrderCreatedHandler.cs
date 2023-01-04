using MediatR;

using SH.Bookstore.Shared.Notifications;
using SH.Bookstore.Whs.Host.Contracts.Services;
using SH.Bookstore.Whs.Host.Mongo.Entites;
using SH.Bookstore.Whs.Host.Mongo.Entites.Events;

namespace SH.Bookstore.Whs.Host.EventHandlers;
public class SupplierOrderCreatedHandler : 
    IEventNotificationHandler<EntityCreatedEvent<SupplierOrder>>
{
    private readonly ISupplierOrderProcessor _supplierOrderProcessor;

    public SupplierOrderCreatedHandler(ISupplierOrderProcessor supplierOrderProcessor)
    {
        _supplierOrderProcessor = supplierOrderProcessor;
    }
    public async Task Handle(EventNotification<EntityCreatedEvent<SupplierOrder>> notification, CancellationToken cancellationToken) 
        => await _supplierOrderProcessor.StartAsync(notification.Event.Entity.Id);
}
