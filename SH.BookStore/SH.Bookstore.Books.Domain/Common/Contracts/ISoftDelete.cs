namespace SH.Bookstore.Books.Domain.Common.Contracts;

/// <summary>
/// Сущности реализующие данный интерфейс не удаляются из базы данных, а только помечаются удаленными. 
/// <br/>
/// В дальнейшем в эти сущности не будут фигурировать в запросах
/// </summary>
public interface ISoftDelete
{
    /// <summary>
    /// Дата пометки на удаление
    /// </summary>
    DateTime? DeletedOn { get; set; }

    /// <summary>
    /// Уникальный идентификатор пользователя, который пометил данную сущность на удаление
    /// </summary>
    Guid? DeletedBy { get; set; }
}
