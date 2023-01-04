using Microsoft.AspNetCore.SignalR;

namespace SH.Bookstore.Shared.Notifications;
public class NotificationSender : INotificationSender
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationSender(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }
    public async Task SendToAllAsync(INotificationMessage message, CancellationToken cancellationToken)
        => await _hubContext.Clients.All.SendAsync(NotificationConstants.NotificationFromServer, message.GetType().Name, message, cancellationToken);
}
