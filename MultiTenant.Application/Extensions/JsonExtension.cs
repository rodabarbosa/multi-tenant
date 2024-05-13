using System.Text.Json;
using System.Text.Json.Serialization;

namespace MultiTenant.Application.Extensions;

public static class JsonExtension
{
    private static readonly JsonSerializerOptions _serializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        UnknownTypeHandling = JsonUnknownTypeHandling.JsonElement
    };

    /// <summary>
    /// Converts an object to a JSON string using the specified serialization options.
    /// </summary>
    /// <param name="obj">The object to convert to JSON.</param>
    /// <returns>The JSON string representation of the object.</returns>
    public static string ToJson(this object obj)
    {
        return JsonSerializer.Serialize(obj, _serializerOptions);
    }

    /// <summary>
    /// Deserializes a JSON string to an object of type T.
    /// </summary>
    /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
    /// <param name="json">The JSON string to deserialize.</param>
    /// <returns>An object of type T if the JSON string is not null or empty, otherwise null.</returns>
    public static T? FromJson<T>(this string? json)
    {
        if (string.IsNullOrEmpty(json?.Trim()))
            return default;

        return JsonSerializer.Deserialize<T>(json.Trim(), _serializerOptions);
    }
}
