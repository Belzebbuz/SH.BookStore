using Microsoft.Extensions.Options;

using MongoDB.Bson;
using MongoDB.Driver;

using SH.Bookstore.Orders.Host.Contracts;
using SH.Bookstore.Orders.Host.Mongo.Entities;
using SH.Bookstore.Shared.Wrapper;

namespace SH.Bookstore.Orders.Host.Mongo.Repositories;
internal class ClientOrderRepository : IClientOrdersRepository
{
    private readonly IMongoCollection<ClientOrder> _ordersCollection;
    

    public ClientOrderRepository(IOptions<MongoSettings> options)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        var mongoDataBase = mongoClient.GetDatabase(options.Value.DatabaseName);
        _ordersCollection = mongoDataBase.GetCollection<ClientOrder>(options.Value.ClientOrdersCollection);
    }

    public async Task CreateAsync(ClientOrder clientOrder)
        => await _ordersCollection.InsertOneAsync(clientOrder);
    public async Task DeleteAsync(string id)
        => await _ordersCollection.DeleteOneAsync(x => x.Id == id);
    public async Task<ClientOrder> GetClientOrderAsync(string id)
        => await _ordersCollection.Find(x => x.Id == id).SingleOrDefaultAsync();

    public async Task UpdateAsync(string id, ClientOrder clientOrder)
        => await _ordersCollection.ReplaceOneAsync(x => x.Id == id, clientOrder);

    public async Task<PaginatedResult<ClientOrder>> GetClientOrdersAsync(Guid clientId, int page, int itemsPerPage)
    {
        var filterBuilder = Builders<ClientOrder>.Filter;
        var filter = filterBuilder.Eq(x => x.CreatedBy, clientId);
        var totalCount = await _ordersCollection.CountDocumentsAsync(filter);
        var result = await _ordersCollection.Find(filter)
            .SortByDescending(x => x.CreatedOn)
            .Skip((page - 1) * itemsPerPage)
            .Limit(itemsPerPage)
            .ToListAsync();
        return PaginatedResult<ClientOrder>.Success(result, (int)totalCount, page, itemsPerPage);
    }
}
