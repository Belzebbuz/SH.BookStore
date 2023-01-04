using Throw;

namespace SH.Bookstore.Whs.Host.Mongo.Entites;

public class OrderBook
{
    public Guid BookId { get; private set; }
    public int Count { get; private set; }

    private OrderBook()
    {
    }

    public static OrderBook Create(Guid bookId, int count)
    {
        return new()
        {
            BookId = bookId.Throw().IfEquals(Guid.Empty),
            Count = count.Throw().IfLessThan(1)
        };
    }
}