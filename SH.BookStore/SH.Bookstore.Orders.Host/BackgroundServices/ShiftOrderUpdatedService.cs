using Mapster;

using Microsoft.Extensions.Options;

using SH.Bookstore.Orders.Host.Contracts;
using SH.Bookstore.Orders.Host.Mongo.Entities;
using SH.Bookstore.Shared.Messaging;
using SH.Bookstore.Shared.Messaging.Pulsar;

using Throw;

namespace SH.Bookstore.Orders.Host.BackgroundServices;
internal class ShiftOrderUpdatedService : BackgroundService
{
    private readonly IMessageSubscriber _messageSubscriber;
    private readonly IClientOrdersRepository _clientOrdersRepository;
    private readonly string _shiftOrderUpdatedTopic;

    public ShiftOrderUpdatedService(
        IMessageSubscriber messageSubscriber, 
        IOptions<PulsarSettings> options,
        IClientOrdersRepository clientOrdersRepository)
    {
        _messageSubscriber = messageSubscriber;
        _clientOrdersRepository = clientOrdersRepository;
        _shiftOrderUpdatedTopic = options.Value.ShiftOrderUpdatedTopic;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        => await _messageSubscriber.SubscribeAsync<ShiftOrderUpdatedMessage>(_shiftOrderUpdatedTopic, async message =>
        {
            var clientOrder = await _clientOrdersRepository.GetClientOrderAsync(message.ClientOrderId);
            clientOrder.ThrowIfNull();
            switch (message.State)
            {
                case ShiftOrderState.New:
                    break;
                case ShiftOrderState.WaitingSupplierOrder:
                    clientOrder.SetState(OrderState.WaitingForSupplier);
                    break;
                case ShiftOrderState.Picking:
                    clientOrder.SetState(OrderState.Picking);
                    break;
                case ShiftOrderState.OnTheWay:
                    clientOrder.SetState(OrderState.OnTheWay);
                    break;
                case ShiftOrderState.Delivered:
                    clientOrder.SetState(OrderState.Delivered);
                    break;
                default:
                    break;
            }
            
            await _clientOrdersRepository.UpdateAsync(clientOrder.Id, clientOrder);
        });
}

internal record ShiftOrderUpdatedMessage(string ClientOrderId, ShiftOrderState State) : IMessage;

public enum ShiftOrderState
{
    New,
    WaitingSupplierOrder,
    Picking,
    OnTheWay,
    Delivered
}