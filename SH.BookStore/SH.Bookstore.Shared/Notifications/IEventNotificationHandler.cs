using MediatR;

namespace SH.Bookstore.Shared.Notifications;
public interface IEventNotificationHandler<TEvent> : INotificationHandler<EventNotification<TEvent>>
    where TEvent : IEvent
{
}
