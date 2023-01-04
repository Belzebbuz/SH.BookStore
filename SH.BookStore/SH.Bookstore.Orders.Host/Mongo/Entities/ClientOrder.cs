using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SH.Bookstore.Orders.Host.Mongo.Entities;
public class ClientOrder
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; private set; }
    public Guid CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public decimal TotalPrice { get; private set; }
    public List<OrderBook> Books { get; private set; }
    public OrderState State { get; private set; }

    public string TargetAddress { get; private set; }

    private ClientOrder()
    {
    }

    public static ClientOrder Create(Guid createdBy, List<OrderBook> books, string targetAddress)
    {
        return new()
        {
            CreatedBy = createdBy,
            Books = books,
            TotalPrice = books.Sum(x => x.Price),
            State = OrderState.Created,
            CreatedOn = DateTime.UtcNow,
            TargetAddress = targetAddress
        };
    }

    public void SetState(OrderState state) => State = state;
}

