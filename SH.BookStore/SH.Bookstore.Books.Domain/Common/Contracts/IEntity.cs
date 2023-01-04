namespace SH.Bookstore.Books.Domain.Common.Contracts;

/// <summary>
/// Базовый интерфейс для всех сущностей в Domain
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Список событий сущности, которые будут опубликованы после упешного вызова SaveChangesAsync
    /// </summary>
    public List<DomainEvent> DomainEvents { get; }
}

/// <summary>
/// Базовый интерфейс для всех сущностей в Domain
/// </summary>
/// <typeparam name="TId">Тип уникального идентификатора сущности</typeparam>
public interface IEntity<TId> : IEntity
{
    /// <summary>
    /// Уникальный идентификатор 
    /// </summary>
    public TId Id { get; }

}
