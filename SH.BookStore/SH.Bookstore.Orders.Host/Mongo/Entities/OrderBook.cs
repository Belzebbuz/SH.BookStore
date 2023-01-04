namespace SH.Bookstore.Orders.Host.Mongo.Entities;

public class OrderBook
{
    public Guid BookId { get; set; }
    public uint Count { get; set; }
    public decimal Price { get; set; }
}

