using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

using SH.Bookstore.Shared.Messaging;
using SH.Bookstore.Shared.Messaging.Pulsar;
using SH.Bookstore.Shared.Notifications;
using SH.Bookstore.Whs.Host.Mongo.Entites;
using SH.Bookstore.Whs.Host.Mongo.Entites.Events;

namespace SH.Bookstore.Whs.Host.EventHandlers;
public class ShiftOrderUpdatedHandler :
    IEventNotificationHandler<EntityUpdatedEvent<ShiftOrder>>
{
    private readonly IMessagePublisher _messagePublisher;
    private readonly string _shiftOrderUpdatedTopic;

    public ShiftOrderUpdatedHandler(IMessagePublisher messagePublisher, IOptions<PulsarSettings> options)
    {
        _messagePublisher = messagePublisher;
        _shiftOrderUpdatedTopic = options.Value.ShiftOrderUpdatedTopic;
    }
    public async Task Handle(EventNotification<EntityUpdatedEvent<ShiftOrder>> notification, CancellationToken cancellationToken)
        => await _messagePublisher.PublishAsync(
            _shiftOrderUpdatedTopic,
            new ShiftOrderUpdatedMessage(notification.Event.Entity.ClientOrderId, notification.Event.Entity.State));
}

internal record ShiftOrderUpdatedMessage(string ClientOrderId, ShiftOrderState State) : IMessage;