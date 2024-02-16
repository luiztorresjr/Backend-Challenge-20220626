using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping.Infra.Services
{
    public class CacheMemoryService : ICacheRedisService
    {
        private readonly IMemoryCache _memoryCache;
        public CacheMemoryService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void Delete<T>(string key)
        {
            _memoryCache.Remove(key);
        }

        public T Get<T>(string key)
        {
            var cache = _memoryCache.Get<T>(key);
            return cache;
        }

        public List<T> GetAll<T>(string key)
        {
            var cache = _memoryCache.Get<List<T>>(key);
            return cache;
        }

        public void Set<T>(string key, T value)
        {
            _memoryCache.Set(key, value);
        }

        public void Set<T>(string key, List<T> values)
        {
            values.ForEach(x => _memoryCache.Set(key, x));
        }

        public void SetAll<T>(string key, List<T> value)
        {
            if(value!= null)
                value.ForEach(x => _memoryCache.Set(key, x));
        }
    }
}
