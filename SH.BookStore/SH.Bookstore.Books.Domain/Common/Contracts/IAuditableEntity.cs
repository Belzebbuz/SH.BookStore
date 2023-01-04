namespace SH.Bookstore.Books.Domain.Common.Contracts;

/// <summary>
/// Базовый интерфейс для сущностей нуждающихся в аудите
/// <br/>
/// Инфомация об изменении/создании сущностей, реализующих этот интерфейс, будет заносится в базу данных при вызове SaveChangesAsync
/// </summary>
public interface IAuditableEntity
{
    /// <summary>
    /// Уникальный идентификатор пользователя, который создал данную сущность
    /// </summary>
    public Guid CreatedBy { get; set; }

    /// <summary>
    /// Дата создания сущности
    /// </summary>
    public DateTime CreatedOn { get; }

    /// <summary>
    /// Уникальный идентификатор пользователя, который внес последние изменения
    /// </summary>
    public Guid LastModifiedBy { get; set; }

    /// <summary>
    /// Дата последнего изменения
    /// </summary>
    public DateTime? LastModifiedOn { get; set; }
}