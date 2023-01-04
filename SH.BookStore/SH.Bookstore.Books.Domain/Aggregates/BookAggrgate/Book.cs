using SH.Bookstore.Books.Domain.Aggregates.AuthorAggregate;
using SH.Bookstore.Books.Domain.Aggregates.BookAggrgate.Entites;
using SH.Bookstore.Books.Domain.Aggregates.TagAggregate;
using SH.Bookstore.Books.Domain.Common.Base;
using SH.Bookstore.Books.Domain.Common.Contracts;
using SH.Bookstore.Books.Domain.Common.Events;
using SH.Bookstore.Books.Domain.Mongo;

using Throw;

namespace SH.Bookstore.Books.Domain.Aggregates.BookAggrgate;
public sealed class Book : AuditableEntity<Guid>, IAggregateRoot, IMongoEntity<MgBook>
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string? ImageUrl { get; private set; }
    public IReadOnlyCollection<Tag> Tags => _tags;
    private List<Tag> _tags = default!;
    public IReadOnlyCollection<Author> Authors => _authors;
    private List<Author> _authors = default!;
    public IReadOnlyCollection<Price> Prices => _prices;
    private List<Price> _prices = default!;

    private Book()
    {
    }

    public static Book Create(
        string title,
        string description,
        string? imageUrl,
        IEnumerable<Tag> tags,
        IEnumerable<Author> authors,
        decimal priceValue,
        DateTime actualDatePrice)
    {
        actualDatePrice.Throw("Initial actual price must be equals or less than current date!")
            .IfGreaterThan(DateTime.UtcNow);
        var book = new Book()
        {
            Title = title.ThrowIfNull().IfEmpty(),
            Description = description.ThrowIfNull().IfEmpty(),
            ImageUrl = imageUrl,
            _tags = tags.ToList(),
            _authors = authors.ToList(),
        };
        book.AddPrice(actualDatePrice,priceValue);
        book.DomainEvents.Add(EntityCreatedEvent.WithEntity(book));
        return book;
    }

    public void AddPrice(DateTime actualDate, decimal priceValue)
    {
        if (_prices == null)
            _prices = new List<Price>();
        _prices.Add(Price.Create(actualDate, priceValue));
    }
}
