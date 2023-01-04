
using Microsoft.Extensions.Options;

using SH.Bookstore.Orders.Host.Contracts;
using SH.Bookstore.Orders.Host.Mongo.Entities;
using SH.Bookstore.Shared.Common.Services.Pagination;
using SH.Bookstore.Shared.Identity.RequestCurrentUser;
using SH.Bookstore.Shared.Messaging;
using SH.Bookstore.Shared.Messaging.Pulsar;
using SH.Bookstore.Shared.Wrapper;

using Throw;

using IResult = SH.Bookstore.Shared.Wrapper.IResult;

namespace SH.Bookstore.Orders.Host.Services;
internal class ClientOrderService : IClientOrderService
{
    private readonly IClientOrdersRepository _orderRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IPaginationService _paginationService;
    private readonly IBookRepository _bookRepository;
    private readonly IMessagePublisher _messagePublisher;
    private readonly PulsarSettings _messageSettings;

    public ClientOrderService(IClientOrdersRepository repository,
                              ICurrentUser currentUser,
                              IPaginationService paginationService, 
                              IBookRepository bookRepository,
                              IMessagePublisher messagePublisher,
                              IOptions<PulsarSettings> options)
    {
        _orderRepository = repository;
        _currentUser = currentUser;
        _paginationService = paginationService;
        _bookRepository = bookRepository;
        _messagePublisher = messagePublisher;
        _messageSettings = options.Value;
    }
    public async Task<IResult> CreateAsync(CreateClientOrderRequest request)
    {
        var catalogBooks = await _bookRepository.GetBooksAsync(request.Books.Select(x => x.BookId));
        catalogBooks.Count.Throw().IfNotEquals(request.Books.Count());

        var orderedBooks = catalogBooks.Select(x => new OrderBook()
        {
            BookId = x.Id,
            Count = request.Books.First(ob => ob.BookId == x.Id).Count,
            Price = x.Price * request.Books.First(ob => ob.BookId == x.Id).Count
        }).ToList();

        var clientOrder = ClientOrder.Create(_currentUser.GetUserId(), orderedBooks, request.Address);
        await _orderRepository.CreateAsync(clientOrder);
        return Result.Success();
    }

    public async Task<PaginatedResult<ClientOrder>> GetClientOrdersAsync()
    {
        var clientId = _currentUser.GetUserId();
        return await _orderRepository.GetClientOrdersAsync(clientId, _paginationService.Page, _paginationService.ItemsPerPage);
    }

    public async Task<IResult> ConfirmClientOrderAsync(string id)
    {
        var order = await _orderRepository.GetClientOrderAsync(id);
        order.ThrowIfNull().IfFalse(x => x.State == OrderState.Created);
        order.SetState(OrderState.ConfirmTotalPrice); 
        await _orderRepository.UpdateAsync(id, order);

        var createShiftOrderMessage = new CreateShiftOrderMessage(id, order.Books.Select(x => new ShiftBook(x.BookId, x.Count)), order.TargetAddress);

        await _messagePublisher.PublishAsync(_messageSettings.ShiftOrderCreateTopic, createShiftOrderMessage);
        return Result.Success();
    }
}

internal record CreateShiftOrderMessage(string ClientOrderId, IEnumerable<ShiftBook> ShiftBooks, string Address) : IMessage;
internal record ShiftBook(Guid BookId, uint Count);