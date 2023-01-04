using SH.Bookstore.Books.Domain.Common.Base;

using Throw;

namespace SH.Bookstore.Books.Domain.Aggregates.BookAggrgate.Entites;
public class Price : BaseEntity<Guid>
{
    public DateTime ActualDate { get; private set; }
    public decimal Value { get; private set; }
    private Price()
    {
    }
    internal static Price Create(DateTime actualDate, decimal value)
    {
        return new()
        {
            ActualDate = actualDate,
            Value = value.Throw().IfLessThan(0),
        };
    }
}
