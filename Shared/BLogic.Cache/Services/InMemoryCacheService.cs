using Microsoft.Extensions.Caching.Memory;

namespace BLogic.Cache.Services;

public class InMemoryCacheService(IMemoryCache memoryCache) : ICacheService
{
    private readonly IMemoryCache _memoryCache = memoryCache;
    private readonly HashSet<string> _keys = new();

    public void ForceRemove(string key)
    {
        _memoryCache.Remove(key);
        _keys.Remove(key);
    }

    public void ClearAll()
    {
        foreach(var key in _keys)
        {
            ForceRemove(key);
        }
        _keys.Clear();
    }

    public IEnumerable<string> GetAllKeys() => _keys;

    public async Task<T> GetOrCreateAsync<T>(string key, Func<ICacheEntry, Task<T>> func)
    {
        var value = await _memoryCache.GetOrCreateAsync(key, func);
        _keys.Add(key);
        return value;
    }

    public T GetOrCreate<T>(string key, Func<ICacheEntry, T> func)
    {
        var value = _memoryCache.GetOrCreate(key, func);
        _keys.Add(key);
        return value;
    }

    public void Set<T>(string key, Func<ICacheEntry, T> func)
    {
        _keys.Add(key);
        _memoryCache.Set(key, func);
    }

    public void Set<T>(string key, T value)
    {
        _keys.Add(key);
        _memoryCache.Set(key, value);
    }
    
    public void Set<T>(string key, T value, MemoryCacheEntryOptions options)
    {
        _keys.Add(key);
        _memoryCache.Set(key, value, options);
    }
    
    public void Set<T>(string key, T value, TimeSpan timeSpan)
    {
        _keys.Add(key);
        _memoryCache.Set(key, value, timeSpan);
    }

    public bool TryGetCache<T>(string key, out T value)
    {
        if (_memoryCache.TryGetValue(key, out var cachedValue) && cachedValue is T)
        {
            value = (T)cachedValue;
            return true;
        }

        value = default!;
        return false;
    }

    public T Get<T>(string key)
    {
        return _memoryCache.Get<T>(key);
    }
}