using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using SH.Bookstore.Whs.Host.Mongo.Entites.Base;
using SH.Bookstore.Whs.Host.Mongo.Entites.Events;

namespace SH.Bookstore.Whs.Host.Mongo.Entites;
public class SupplierOrder : BaseMongoEntity
{
    public string ShiftOrderId { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public List<OrderBook> Books { get; private set; }
    public SupplierOrderState State { get; private set; }

    private SupplierOrder()
    {
    }
    public static SupplierOrder Create(string shiftOrderId, List<OrderBook> books)
    {
        var order =  new SupplierOrder()
        {
           ShiftOrderId= shiftOrderId,
           Books = books,
           CreatedOn= DateTime.Now,
           State = SupplierOrderState.New
        };
        order.DomanEvents.Add(EntityCreatedEvent.WithEntity(order));
        return order;
    }

    internal void SetState(SupplierOrderState state)
    {
        
        State = state;
        if (State == SupplierOrderState.Delivered)
            DomanEvents.Add(new SupplierDeliveredOrderEvent(this));
    }
}

public enum SupplierOrderState
{
    New,
    Picking,
    OnTheWay,
    Delivered
}