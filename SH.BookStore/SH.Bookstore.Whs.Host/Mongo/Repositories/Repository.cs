using System.Linq.Expressions;

using Microsoft.Extensions.Options;

using MongoDB.Driver;

using SH.Bookstore.Shared.Notifications;
using SH.Bookstore.Whs.Host.Contracts.Repositories;
using SH.Bookstore.Whs.Host.Mongo.Entites;
using SH.Bookstore.Whs.Host.Mongo.Entites.Base;
using SH.Bookstore.Whs.Host.Mongo.Entites.Events;

namespace SH.Bookstore.Whs.Host.Mongo.Repositories;
internal class Repository<T> : IRepository<T> 
    where T : class, IMongoEntity
{
    private readonly IMongoCollection<T> _collection;
    private readonly IServiceProvider _serviceProvider;

    public Repository(IOptions<MongoSettings> options, IServiceProvider serviceProvider)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        var mongoDataBase = mongoClient.GetDatabase(options.Value.DatabaseName);
        _collection = mongoDataBase.GetCollection<T>($"{typeof(T).Name}-collection");
        _serviceProvider = serviceProvider;
    }
    public async Task CreateAsync(T entity)
    {
        await _collection.InsertOneAsync(entity);
        await PublishDomainEventsAsync(entity);
    }

    public async Task DeleteAsync(string id) 
        => await _collection.DeleteOneAsync(x => x.Id == id);
    public async Task<T> GetByIdAsync(string id) 
        => await _collection.Find(x => x.Id == id).SingleOrDefaultAsync();
    public async Task<List<T>> GetByFilterAsync(Expression<Func<T,bool>> expression) 
        => await _collection.Find(expression).ToListAsync();

    public async Task<T> GetSingleByFilterAsync(Expression<Func<T, bool>> expression)
        => await _collection.Find(expression).SingleOrDefaultAsync();
    public async Task UpdateAsync(string id, T entity)
    {
        await _collection.ReplaceOneAsync(x => x.Id == id, entity);
        await PublishDomainEventsAsync(entity);
    }

    public async Task PublishDomainEventsAsync(T entity)
    {
        var publisher = _serviceProvider.GetRequiredService<IEventPublisher>();
        foreach (var @event in entity.DomanEvents)
        {
            await publisher.PublishAsync(@event);
        }
    }
}
