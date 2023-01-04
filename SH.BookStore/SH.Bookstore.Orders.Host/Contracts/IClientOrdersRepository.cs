using SH.Bookstore.Orders.Host.Mongo.Entities;
using SH.Bookstore.Shared.Common.DI;
using SH.Bookstore.Shared.Wrapper;

namespace SH.Bookstore.Orders.Host.Contracts;
internal interface IClientOrdersRepository : ISingletonService
{
    public Task CreateAsync(ClientOrder clientOrder);
    public Task<ClientOrder> GetClientOrderAsync(string id);
    public Task<PaginatedResult<ClientOrder>> GetClientOrdersAsync(Guid clientId, int page, int itemsPerPage);
    public Task DeleteAsync(string id);
    public Task UpdateAsync(string id, ClientOrder clientOrder);
}
