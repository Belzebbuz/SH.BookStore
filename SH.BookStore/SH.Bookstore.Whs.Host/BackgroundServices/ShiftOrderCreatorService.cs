using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;
using SH.Bookstore.Shared.Messaging.Pulsar;
using SH.Bookstore.Shared.Messaging;
using SH.Bookstore.Whs.Host.Mongo.Entites;
using SH.Bookstore.Whs.Host.Contracts.Services;
using SH.Bookstore.Whs.Host.Contracts.Repositories;
using Throw;

namespace SH.Bookstore.Whs.Host.BackgroundServices;
internal class ShiftOrderCreatorService : BackgroundService
{
    private readonly string _shiftOrderCreateTopic;
    private readonly IMessageSubscriber _messageSubscriber;
    private readonly IRepository<ShiftOrder> _shiftOrderRepository;
    private readonly IShiftOrderProcessor _shiftOrderProcessor;

    public ShiftOrderCreatorService(IMessageSubscriber messageSubscriber,
        IOptions<PulsarSettings> options,
        IRepository<ShiftOrder> shiftOrderRepository,
        IShiftOrderProcessor shiftOrderProcessor)
    {
        _shiftOrderCreateTopic = options.Value.ShiftOrderCreateTopic;
        _messageSubscriber = messageSubscriber;
        _shiftOrderRepository = shiftOrderRepository;
        _shiftOrderProcessor = shiftOrderProcessor;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        => await _messageSubscriber.SubscribeAsync<CreateShiftOrderMessage>(_shiftOrderCreateTopic, async message => 
        {
            var shiftOrder = ShiftOrder.Create(
                message.ClientOrderId,
                message.ShiftBooks.Select(x => OrderBook.Create(x.BookId, x.Count)).ToList(),
                message.Address);
            await _shiftOrderRepository.CreateAsync(shiftOrder);
            await _shiftOrderProcessor.StartAsync(shiftOrder.Id.ThrowIfNull().IfEmpty());
        });
}

public record CreateShiftOrderMessage(string ClientOrderId, IEnumerable<ShiftBook> ShiftBooks, string Address) : IMessage;
public record ShiftBook(Guid BookId, int Count);