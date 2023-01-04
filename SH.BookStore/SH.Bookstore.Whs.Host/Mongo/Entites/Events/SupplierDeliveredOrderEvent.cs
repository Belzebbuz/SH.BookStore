using SH.Bookstore.Whs.Host.Mongo.Entites.Base;

namespace SH.Bookstore.Whs.Host.Mongo.Entites.Events;
public class SupplierDeliveredOrderEvent : DomainEvent
{
    internal SupplierOrder Order { get; private set; }
    internal SupplierDeliveredOrderEvent (SupplierOrder order)  => Order = order;
}
