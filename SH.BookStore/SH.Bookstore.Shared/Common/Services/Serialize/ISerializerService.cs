using SH.Bookstore.Shared.Common.DI;

namespace SH.Bookstore.Shared.Common.Services.Serialize;

/// <summary>
/// Сервис сериализации json объектов
/// </summary>
public interface ISerializerService : ITransientService
{
    /// <summary>
    /// Сериализация объекта в строку json
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns>Строка json</returns>
    public string Serialize<T>(T obj);

    /// <summary>
    /// Десериализация json строки в объект
    /// </summary>
    /// <typeparam name="T">Целевой тип десериализации</typeparam>
    /// <param name="text">Строка json</param>
    /// <returns>Объект десериализации</returns>
    public T? Deserialize<T>(string text);

    public byte[] SerializeBytes<T>(T value) where T : class;
    public T? DeserializeBytes<T>(byte[] value) where T : class;


}
