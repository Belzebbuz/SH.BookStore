using SH.Bookstore.Books.Application.Contracts.Queries;
using SH.Bookstore.Books.Domain.Aggregates.BookAggrgate;
using SH.Bookstore.Books.Domain.Common.Contracts;
using SH.Bookstore.Books.Domain.Mongo;
using SH.Bookstore.Shared.Common.DI;
using SH.Bookstore.Shared.Wrapper;

namespace SH.Bookstore.Books.Application.Contracts.Services.Mongo;

public interface IMongoRepository : IScopedService
{
    /// <summary>
    /// Synchronize SQL entity with NoSQL entity
    /// </summary>
    /// <param name="id">SQL entity Id</param>
    /// <returns></returns>
    public Task AddOrUpdateAsync(Guid id);
}
public interface IMongoRepository<TSourceEntity, TMongo> : IMongoRepository
    where TSourceEntity : class, IMongoEntity<TMongo>, IEntity
    where TMongo : class, IMongo 
{
    public Task<PaginatedResult<TMongo>> GetByFilterAsync(IQuery queryFilter, int page, int itemsPerPage);
    public Task<IResult<TMongo>> GetByIdAsync(Guid id);
}
