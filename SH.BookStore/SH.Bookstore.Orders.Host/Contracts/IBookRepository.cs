using SH.Bookstore.Orders.Host.Mongo.Entities;
using SH.Bookstore.Shared.Common.DI;

namespace SH.Bookstore.Orders.Host.Contracts;

internal interface IBookRepository : IScopedService
{
    public Task<Book> GetBookAsync(Guid id);
    public Task<List<Book>> GetBooksAsync(IEnumerable<Guid> ids);
}