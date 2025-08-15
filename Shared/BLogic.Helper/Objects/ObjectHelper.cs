using System.Text.Json;
using BLogic.Helper.Extensions;

namespace BLogic.Helper.Objects;


public static class ObjectHelper
{
    public static T DeepClone<T>(this T obj)
    {
        var json = JsonSerializer.Serialize(obj);
        return JsonExtension.TryDeserialize<T>(json);
    }
    
    public static void Clear(this object obj)
    {
        var stringProps = typeof(object).GetProperties()
            .Where(p => p.CanRead && p.CanWrite && p.PropertyType == typeof(string));

        foreach (var prop in stringProps)
        {
            var val = (string)prop.GetValue(obj)!;
            prop.SetValue(obj, val.Trim());
        }
    }
}