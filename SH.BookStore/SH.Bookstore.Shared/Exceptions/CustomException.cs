using System.Net;

namespace SH.Bookstore.Shared.Exceptions;

/// <summary>
/// Базовый класс для внутренних исключений
/// </summary>
public abstract class CustomException : Exception
{
    /// <summary>
    /// Список ошибок
    /// </summary>
    public List<string>? ErrorMessages { get; }

    /// <summary>
    /// HTTP Статус код для ответа клиенту
    /// </summary>
    public HttpStatusCode StatusCode { get; }

    protected CustomException(string message, List<string>? errors = default, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        : base(message)
    {
        ErrorMessages = errors;
        StatusCode = statusCode;
    }
}
