using Microsoft.Extensions.Options;

using SH.Bookstore.Books.Domain.Aggregates.BookAggrgate;
using SH.Bookstore.Books.Domain.Common.Events;
using SH.Bookstore.Shared.Messaging;
using SH.Bookstore.Shared.Messaging.Pulsar;
using SH.Bookstore.Shared.Notifications;

namespace SH.Bookstore.Books.Infrastructure.EventHandlers;
public class BookCreatedEventHandler : IEventNotificationHandler<EntityCreatedEvent<Book>>
{
    private readonly IMessagePublisher _messagePublisher;
    private readonly string _bookCreatedTopic;

    public BookCreatedEventHandler(IMessagePublisher messagePublisher, IOptions<PulsarSettings> options)
    {
        _messagePublisher = messagePublisher;
        _bookCreatedTopic = options.Value.BookCreatedTopic;
    }
    public async Task Handle(EventNotification<EntityCreatedEvent<Book>> notification, CancellationToken cancellationToken) 
        => await _messagePublisher.PublishAsync(_bookCreatedTopic, new BookCreatedMessage(notification.Event.Entity.Id));
}

public record BookCreatedMessage(Guid BookId) : IMessage;
