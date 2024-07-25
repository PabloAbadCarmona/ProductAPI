using Microsoft.Extensions.Caching.Memory;
using ProductAPI.Interfaces.Services;

namespace ProductAPI.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private const string CacheKey = "ProductStatusDictionary";

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void SetProductStatusDictionary(Dictionary<int, string> statusDictionary)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            _memoryCache.Set(CacheKey, statusDictionary, cacheEntryOptions);
        }

        public Dictionary<int, string>? GetProductStatusDictionary()
        {
            _memoryCache.TryGetValue(CacheKey, out Dictionary<int, string>? statusDictionary);
            return statusDictionary;
        }

        public string? GetStatusName(int statusKey)
        {
            var statusDictionary = GetProductStatusDictionary();
            if (statusDictionary is not null && statusDictionary.TryGetValue(statusKey, out var statusName))
            {
                return statusName;
            }

            return null;
        }
    }
}
