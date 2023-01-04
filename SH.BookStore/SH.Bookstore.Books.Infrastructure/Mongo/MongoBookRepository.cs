using Mapster;

using Microsoft.Extensions.Options;

using MongoDB.Driver;

using SH.Bookstore.Books.Application.Books.Queries;
using SH.Bookstore.Books.Application.Contracts.Queries;
using SH.Bookstore.Books.Application.Contracts.Repository;
using SH.Bookstore.Books.Application.Contracts.Services.Mongo;
using SH.Bookstore.Books.Application.Specifications.BookSpecifications;
using SH.Bookstore.Books.Domain.Aggregates.BookAggrgate;
using SH.Bookstore.Books.Domain.Mongo;
using SH.Bookstore.Books.Infrastructure.Mongo.Settings;
using SH.Bookstore.Shared.Wrapper;

using Throw;

namespace SH.Bookstore.Books.Infrastructure.Mongo;
public class MongoBookRepository : IMongoRepository<Book, MgBook>
{
    private readonly IMongoCollection<MgBook> _booksCollection;
    private readonly IReadRepository<Book> _readRepository;

    public MongoBookRepository(IOptions<MongoSettings> options, IReadRepository<Book> readRepository)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        var mongoDataBase = mongoClient.GetDatabase(options.Value.DatabaseName);
        _booksCollection = mongoDataBase.GetCollection<MgBook>(options.Value.BooksCoollection);
        _readRepository = readRepository;
    }
    public async Task AddOrUpdateAsync(Guid id)
    {
        var book = await _readRepository.SingleOrDefaultAsync(BooksSpec.GetById(id, ignoreFilters: true));
        book.ThrowIfNull($"Book with id: {id} not found!");

        if(book.DeletedOn.HasValue)
        {
            await _booksCollection.DeleteOneAsync(x => x.Id == book.Id);
            return;
        }

        var mongoBook = book.Adapt<MgBook>();

        var mgBook = await _booksCollection.Find(x => x.Id == mongoBook.Id).SingleOrDefaultAsync();
        if (mgBook != null)
        {
            await _booksCollection.ReplaceOneAsync(x => x.Id == mongoBook.Id, mongoBook);
            return;
        }

        await _booksCollection.InsertOneAsync(mongoBook);
    }

    public async Task<PaginatedResult<MgBook>> GetByFilterAsync(IQuery queryFilter, int page, int itemsPerPage)
    {
        var request = queryFilter as SearchBooksQuery;
        request.ThrowIfNull();
        var builder = Builders<MgBook>.Filter;
        var filterTitle = builder.Regex(x => x.Title, $"^{request.Title}.*");
        var filterAuthor = builder.ElemMatch(x => x.Authors, Builders<MgAuthor>.Filter.Regex(x => x.Name, $"^{request.Author}.*"));
        var filterTag = builder.ElemMatch(x => x.Tags, Builders<MgTag>.Filter.Regex(x => x.Title, $"^{request.Tag}.*"));
        var filter = filterTitle & filterAuthor & filterTag;
        var totalCount = await _booksCollection.CountDocumentsAsync(filter);

        var query = _booksCollection.Find(filter)
            .Skip((page - 1) * itemsPerPage)
            .Limit(itemsPerPage);

        SetOrder(request.OrderBy, query);
        var result = await query.ToListAsync();
        return PaginatedResult<MgBook>.Success(result, (int)totalCount, page, itemsPerPage);
    }
    private void SetOrder(Order orderBy, IFindFluent<MgBook, MgBook> query)
    {
        switch (orderBy)
        {
            case Order.CreateDateAscending:
                query.SortBy(x => x.CreatedOn);
                break;
            case Order.CreateDateDescending:
                query.SortByDescending(x => x.CreatedOn);
                break;
            case Order.TitleAscending:
                query.SortBy(x => x.Title);
                break;
            case Order.TitleDescending:
                query.SortByDescending(x => x.Title);
                break;
            case Order.PriceDescending:
                query.SortByDescending(x => x.Price);
                break;
            case Order.PriceAscending:
                query.SortBy(x => x.Price);
                break;
            default:
                query.SortBy(x => x.Title);
                break;
        }
    }
    public async Task<IResult<MgBook>> GetByIdAsync(Guid id)
        => Result<MgBook>.Success(await _booksCollection.Find(x => x.Id == id).SingleOrDefaultAsync());
}
