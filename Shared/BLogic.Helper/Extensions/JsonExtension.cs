using System.Text.Json;
using System.Text.Json.Serialization;

namespace BLogic.Helper.Extensions;

public static class JsonExtension
{
    private static T Deserialize<T>(string jsonString)
    {
        if (string.IsNullOrWhiteSpace(jsonString))
            return Activator.CreateInstance<T>();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
        };

        T obj = JsonSerializer.Deserialize<T>(jsonString, options) ?? Activator.CreateInstance<T>();
        return obj;
    }

    public static string Serialize(object obj)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        return JsonSerializer.Serialize(obj, options);
    }

    public static T TryDeserialize<T>(string jsonString) {
        try {
            return Deserialize<T>(jsonString);
        }catch (Exception){
            return Activator.CreateInstance<T>();
        }
    }
}