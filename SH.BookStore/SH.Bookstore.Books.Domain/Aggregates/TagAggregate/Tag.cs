using SH.Bookstore.Books.Domain.Aggregates.BookAggrgate;
using SH.Bookstore.Books.Domain.Common.Base;
using SH.Bookstore.Books.Domain.Common.Contracts;
using SH.Bookstore.Books.Domain.Common.Events;

namespace SH.Bookstore.Books.Domain.Aggregates.TagAggregate;
public sealed class Tag : AuditableEntity<Guid>, IAggregateRoot
{
    public string Title { get; private set; }
    public IReadOnlyCollection<Book> Books => _books;
    private HashSet<Book> _books = default!;
    private Tag()
    {
    }

    public static Tag Create(string title)
    {
        var tag = new Tag()
        {
            Title = title
        };
        tag.DomainEvents.Add(EntityCreatedEvent.WithEntity(tag));
        return tag;
    }
}
