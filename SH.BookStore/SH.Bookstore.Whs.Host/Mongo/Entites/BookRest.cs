using SH.Bookstore.Whs.Host.Mongo.Entites.Base;

using Throw;

namespace SH.Bookstore.Whs.Host.Mongo.Entites;
public class BookRest : BaseMongoEntity
{
    public Guid BookId { get; private set; }
    public int FreeCount { get; private set; }
    public int ReservedCount { get; private set; }
    public int OnTheWayCount { get; private set; }
    public int DeliveredCount { get; private set; }
    private BookRest()
    {
    }

    public static BookRest Create(Guid bookId)
    {
        return new() { BookId = bookId };
    }

    public void AddFreeCount(int freeCount)
    {
        freeCount.Throw().IfLessThan(0);
        FreeCount += freeCount;
    }

    public void SetReserveCount(int reserveCount)
    {
        reserveCount.Throw().IfLessThan(0);
        FreeCount = FreeCount - reserveCount;
        FreeCount.Throw().IfLessThan(0);
        ReservedCount = reserveCount;
    }

    public void SetOnTheWayCount(int onTheWayCount)
    {
        onTheWayCount.Throw().IfLessThan(0);
        ReservedCount = ReservedCount - onTheWayCount;
        ReservedCount.Throw().IfLessThan(0);
        OnTheWayCount = onTheWayCount;
    }

    public void SetDeliveredCount(int deliveredCount)
    {
        deliveredCount.Throw().IfLessThan(0);
        OnTheWayCount = OnTheWayCount - deliveredCount;
        OnTheWayCount.Throw().IfLessThan(0);
        DeliveredCount = deliveredCount;
    }
}
