using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.Extensions.Options;

using Throw;

namespace SH.Bookstore.Shared.Common.Services.Serialize;

/// <summary>
/// Реализация <see cref="ISerializerService"/>
/// </summary>
public class SystemJsonService : ISerializerService
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    /// <inheritdoc/>
    public T? Deserialize<T>(string text) => JsonSerializer.Deserialize<T>(text);
    public T? DeserializeBytes<T>(byte[] value) where T : class => JsonSerializer.Deserialize<T>(value, Options);

    /// <inheritdoc/>
    public string Serialize<T>(T obj)
    {
        return JsonSerializer.Serialize(obj)
            .Throw().IfNullOrEmpty(value => value);
    }

    public byte[] SerializeBytes<T>(T value) where T : class => JsonSerializer.SerializeToUtf8Bytes(value, Options);
}

