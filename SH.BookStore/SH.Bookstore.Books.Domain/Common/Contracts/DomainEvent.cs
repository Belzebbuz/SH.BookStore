using SH.Bookstore.Shared.Notifications;

namespace SH.Bookstore.Books.Domain.Common.Contracts;

/// <summary>
/// Базовый класс для событий Domain области
/// </summary>
public abstract class DomainEvent : IEvent
{
    /// <summary>
    /// Время создания события
    /// </summary>
    public DateTime TriggeredOn { get; protected set; } = DateTime.UtcNow;
}
