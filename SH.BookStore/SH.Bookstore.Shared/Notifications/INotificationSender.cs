using SH.Bookstore.Shared.Common.DI;

namespace SH.Bookstore.Shared.Notifications;

public interface INotificationSender : ITransientService
{
    /// <summary>
    /// Отправить оповещение все пользователям
    /// </summary>
    /// <returns></returns>
    public Task SendToAllAsync(INotificationMessage message, CancellationToken cancellationToken);
}
