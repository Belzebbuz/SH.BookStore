using System.ComponentModel.DataAnnotations.Schema;

using SH.Bookstore.Books.Domain.Common.Contracts;

namespace SH.Bookstore.Books.Domain.Common.Base;

/// <summary>
/// Базовый класс сущности
/// </summary>
/// <typeparam name="TId">Тип уникального идентификатора сущности</typeparam>
public abstract class BaseEntity<TId> : IEntity<TId>
{
    /// <inheritdoc/>
    public TId Id { get; protected set; } = default!;

    /// <inheritdoc/>
    [NotMapped]
    public List<DomainEvent> DomainEvents { get; } = new();
}
