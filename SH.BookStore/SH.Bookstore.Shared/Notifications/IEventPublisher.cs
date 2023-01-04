using SH.Bookstore.Shared.Common.DI;

namespace SH.Bookstore.Shared.Notifications;

/// <summary>
/// <see cref="IEventPublisher"/> позволяет опубликовать событие
/// </summary>
public interface IEventPublisher : ITransientService
{
    /// <summary>
    /// Публикация события
    /// </summary>
    /// <param name="event"></param>
    /// <returns></returns>
    public Task PublishAsync(IEvent @event);
}
