using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SH.Bookstore.Orders.Host.Contracts;
using SH.Bookstore.Orders.Host.Mongo.Entities;
using SH.Bookstore.Shared.Wrapper;

using IResult = SH.Bookstore.Shared.Wrapper.IResult;

namespace SH.Bookstore.Orders.Host.Controllers;

[Route("orders")]
[ApiController]
[Authorize]
public class OrderController : ControllerBase
{
    private readonly IClientOrderService _clientOrderService;

    public OrderController(IClientOrderService clientOrderService) => _clientOrderService = clientOrderService;
    [HttpPost]
    public async Task<IResult> CreateAsync(CreateClientOrderRequest request)
        => await _clientOrderService.CreateAsync(request);

    [HttpGet]
    public async Task<PaginatedResult<ClientOrder>> GetClientOrders()
        => await _clientOrderService.GetClientOrdersAsync();

    [HttpGet("{orderId}/confirm")]
    public async Task<IResult> ConfirmOrderAsync(string orderId)
        => await _clientOrderService.ConfirmClientOrderAsync(orderId);
}
