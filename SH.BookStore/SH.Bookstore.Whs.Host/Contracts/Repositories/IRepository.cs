using SH.Bookstore.Shared.Common.DI;
using MongoDB.Driver;
using System.Linq.Expressions;
using SH.Bookstore.Whs.Host.Mongo.Entites.Base;

namespace SH.Bookstore.Whs.Host.Contracts.Repositories;
public interface IRepository<TEntity>
    where TEntity : class, IMongoEntity
{
    public Task CreateAsync(TEntity entity);
    public Task UpdateAsync(string id, TEntity entity);
    public Task DeleteAsync(string id);
    public Task<TEntity> GetByIdAsync(string id);
    public Task<List<TEntity>> GetByFilterAsync(Expression<Func<TEntity, bool>> expression);
    public Task<TEntity> GetSingleByFilterAsync(Expression<Func<TEntity, bool>> expression);
}
