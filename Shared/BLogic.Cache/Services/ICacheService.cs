using Microsoft.Extensions.Caching.Memory;

namespace BLogic.Cache.Services;

public interface ICacheService
{
    void ForceRemove(string key);
    void ClearAll();
    IEnumerable<string> GetAllKeys();
    Task<T> GetOrCreateAsync<T>(string key, Func<ICacheEntry, Task<T>> func);
    T GetOrCreate<T>(string key, Func<ICacheEntry, T> func);
    void Set<T>(string key, Func<ICacheEntry, T> func);
    void Set<T>(string key, T value);
    bool TryGetCache<T>(string key, out T value);
    void Set<T>(string key, T value, MemoryCacheEntryOptions options);
    void Set<T>(string key, T value, TimeSpan timeSpan);
    T Get<T>(string key);
}
