using Microsoft.Extensions.Options;
using MongoDB.Driver;

using SH.Bookstore.Orders.Host.Contracts;
using SH.Bookstore.Orders.Host.Mongo.Entities;

namespace SH.Bookstore.Orders.Host.Mongo.Repositories;
internal class BookRepository : IBookRepository
{
    private readonly IMongoCollection<Book> _booksCollection;
    public BookRepository(IOptions<MongoSettings> options)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        var mongoDataBase = mongoClient.GetDatabase(options.Value.DatabaseName);
        _booksCollection = mongoDataBase.GetCollection<Book>(options.Value.BooksCoollection);
    }
    public async Task<Book> GetBookAsync(Guid id)
        => await _booksCollection.Find(x => x.Id == id).SingleOrDefaultAsync();

    public async Task<List<Book>> GetBooksAsync(IEnumerable<Guid> ids)
        => await _booksCollection.Find(x => ids.Contains(x.Id)).ToListAsync();
}
