using SH.Bookstore.Books.Domain.Common.Contracts;

namespace SH.Bookstore.Books.Domain.Common.Base;

/// <summary>
/// Базовый класс для сущности, требующие проведение аудита изменений своего состояния в базе данных
/// </summary>
/// <typeparam name="TId">Тип уникального идентификатора сущности</typeparam>
public abstract class AuditableEntity<TId> : BaseEntity<TId>, IAuditableEntity, ISoftDelete
{
    /// <inheritdoc/>
    public Guid CreatedBy { get; set; }

    /// <inheritdoc/>
    public DateTime CreatedOn { get; private set; }

    /// <inheritdoc/>
    public DateTime? LastModifiedOn { get; set; }

    /// <inheritdoc/>
    public Guid LastModifiedBy { get; set; }

    /// <inheritdoc/>
    public DateTime? DeletedOn { get; set; }

    /// <inheritdoc/>
    public Guid? DeletedBy { get; set; }

    protected AuditableEntity()
    {
        CreatedOn = DateTime.UtcNow;
        LastModifiedOn = DateTime.UtcNow;
    }
}
