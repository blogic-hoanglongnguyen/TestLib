using System.Collections.Concurrent;
using System.Reflection;

namespace BLogic.Helper.Reflections;

public static class ReflectionCache
{
    private static readonly ConcurrentDictionary<Type, PropertyInfo[]> _propertyCache = new();
    private static readonly ConcurrentDictionary<Type, List<Type>> _typeInheritedFromInterface = new();

    public static PropertyInfo[] GetPublicInstanceProperties(Type type)
    {
        return _propertyCache.GetOrAdd(type, t =>
        {
            return t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        });
    }
    
    public static PropertyInfo? GetPublicInstanceProperty(Type type, string propertyName)
    {
        return GetPublicInstanceProperties(type)
            .FirstOrDefault(p => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
    }
    
    public static TAttribute? GetAttributeFromProperty<TAttribute, TEnum>(TEnum value)
        where TAttribute : Attribute
        where TEnum : Enum
    {
        var type = typeof(TEnum);
        var name = Enum.GetName(type, value);
        if (name == null) return null;

        var field = type.GetField(name);
        return field?.GetCustomAttribute<TAttribute>();
    }
    
    public static List<Type> GetInheritedFromInterface(Assembly assembly, Type itf)
    {
        return _typeInheritedFromInterface.GetOrAdd(itf, t =>
        {
            var result = assembly.GetTypes().Where(t => itf.IsAssignableFrom(t));
            return result.ToList();
        });
    }
    
    public static List<Type> GetInheritedFromInterface(Assembly[] assemblies, Type itf)
    {
        return _typeInheritedFromInterface.GetOrAdd(itf, _ =>
        {
            var result = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => itf.IsAssignableFrom(t));
            return result.ToList();
        });
    }
    

    public static ConcurrentDictionary<Type, PropertyInfo[]> GetAll()
    {
        return _propertyCache;
    }
}