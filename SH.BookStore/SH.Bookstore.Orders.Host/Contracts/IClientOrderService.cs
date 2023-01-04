using SH.Bookstore.Orders.Host.Mongo.Entities;
using SH.Bookstore.Shared.Common.DI;
using SH.Bookstore.Shared.Wrapper;

using IResult = SH.Bookstore.Shared.Wrapper.IResult;

namespace SH.Bookstore.Orders.Host.Contracts;
public interface IClientOrderService : IScopedService
{
    Task<IResult> ConfirmClientOrderAsync(string id);
    public Task<IResult> CreateAsync(CreateClientOrderRequest request);
    public Task<PaginatedResult<ClientOrder>> GetClientOrdersAsync();
}

public record CreateClientOrderRequest(IEnumerable<BookRequest> Books, string Address);

public class BookRequest
{
    public Guid BookId { get; set; }
    public uint Count { get; set; }
}