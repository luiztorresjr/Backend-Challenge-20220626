using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping.Infra.Cache
{
    internal class CacheMemoryService : ICacheService
    {
        private readonly IMemoryCache _cache;

        public CacheMemoryService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public T Get<T>(string key)
        {
            var cache = _cache.Get<T>(key);
            return cache;
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        public void Set<T>(string key, T value)
        {
            _cache.Set(key, value);
        }
    }
}
