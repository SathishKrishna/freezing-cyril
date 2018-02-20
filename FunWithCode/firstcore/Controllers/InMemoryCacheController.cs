using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace firstcore.Controllers
{
    public enum CacheKeys{
        Entry = 0,
        CallbackEntry = 1,
        CallbackMessage = 2,
        DependentCTS = 3,
        Parent = 4,
        Child = 5,
        DependentMessage = 6
    }
    public class InMemoryCacheController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public InMemoryCacheController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        // set a cache value
        public IActionResult CacheTryGetValueSet()
        {
            DateTime cacheEntry;
            // Look for cache key
            if(!_memoryCache.TryGetValue(CacheKeys.Entry, out cacheEntry))
            {
                // key not in cache, so get the data
                cacheEntry = DateTime.Now;

                // set the cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(3));

                // save the cache
                _memoryCache.Set(CacheKeys.Entry, cacheEntry, cacheEntryOptions);
            }
            return View("cache", cacheEntry);
        }

        public IActionResult CacheGetOrCreate()
        {
            var cacheEntry = _memoryCache.GetOrCreate(CacheKeys.Entry, entry => {
                entry.SlidingExpiration = TimeSpan.FromSeconds(3);
                return DateTime.Now;
            });
            return View("cache", cacheEntry);
        }

        public async Task<IActionResult> CacheGetOrCreateAsync()
        {
            var cacheEntry = await _memoryCache.GetOrCreateAsync(CacheKeys.Entry, entry => {
                entry.SlidingExpiration = TimeSpan.FromSeconds(3);
                return Task.FromResult(DateTime.Now);
            });
            return View("cache", cacheEntry);
        }

        public IActionResult CacheGet()
        {
            var cacheEntry = _memoryCache.Get<DateTime?>(CacheKeys.Entry);
            return View("cache", cacheEntry);
        }
    }
}