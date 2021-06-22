using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace API.Services
{
    public interface ITestableCache
    {
        bool TryGet<T>(object key, out T item);
        bool TryGet<T>(object key, out List<T> item);
        void Set<T>(object key, T value);
    }

    public class TestableCache : ITestableCache
    {
        IMemoryCache _memoryCache;
        readonly int _cacheDuration;
       
        public TestableCache(IConfiguration config,IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _cacheDuration = config.GetValue<int>("CacheDuration");
        }

        public void Set<T>(object key,T value) { _memoryCache.Set(key, value,TimeSpan.FromSeconds(_cacheDuration)); }

        public bool TryGet<T>(object key, out T item) { return _memoryCache.TryGetValue(key, out item); }

        public bool TryGet<T>(object key, out List<T> item) { return _memoryCache.TryGetValue(key, out item); }


    }
}
