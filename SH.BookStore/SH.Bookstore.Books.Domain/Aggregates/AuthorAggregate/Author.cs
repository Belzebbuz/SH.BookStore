using SH.Bookstore.Books.Domain.Aggregates.BookAggrgate;
using SH.Bookstore.Books.Domain.Common.Base;
using SH.Bookstore.Books.Domain.Common.Contracts;
using SH.Bookstore.Books.Domain.Common.Events;

using Throw;

namespace SH.Bookstore.Books.Domain.Aggregates.AuthorAggregate;
public sealed class Author : AuditableEntity<Guid>, IAggregateRoot
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public IReadOnlyCollection<Book> Books => _books;
    private HashSet<Book> _books = default!;

    private Author()
    {
    }

    public static Author Create(string name, string description)
    {
        var author = new Author()
        {
            Name = name.ThrowIfNull().IfEmpty(),
            Description = description.ThrowIfNull().IfEmpty()
        };
        author.DomainEvents.Add(EntityCreatedEvent.WithEntity(author));
        return author;
    }
}
