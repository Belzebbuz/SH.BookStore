using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Throw;
using SH.Bookstore.Whs.Host.Mongo.Entites.Base;
using SH.Bookstore.Whs.Host.Mongo.Entites.Events;

namespace SH.Bookstore.Whs.Host.Mongo.Entites;
public class ShiftOrder : BaseMongoEntity
{
    public string ClientOrderId { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public List<OrderBook> Books { get; private set; }
    public ShiftOrderState State { get; private set; }
    public string TargetAddress { get; private set; }
    private ShiftOrder()
    {
    }

    public static ShiftOrder Create(string clientOrderId, List<OrderBook> books, string targetAddress)
    {
        var shiftOrder = new ShiftOrder()
        {
            ClientOrderId = clientOrderId.ThrowIfNull(),
            Books = books.ThrowIfNull().IfCountLessThan(1),
            State = ShiftOrderState.New,
            CreatedOn= DateTime.UtcNow,
            TargetAddress = targetAddress.ThrowIfNull().IfEmpty()
        };
        shiftOrder.DomanEvents.Add(EntityCreatedEvent.WithEntity(shiftOrder));
        return shiftOrder;
    }

    public void SetState(ShiftOrderState state)
    {
        State = state;
        DomanEvents.Add(EntityUpdatedEvent.WithEntity(this));
    }
}

public enum ShiftOrderState
{
    New,
    WaitingSupplierOrder,
    Picking,
    OnTheWay,
    Delivered
}