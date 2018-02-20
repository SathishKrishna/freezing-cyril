using System;
using System.Threading;
using firstcore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace firstcore.Controllers
{
    public class InMemoryCacheDependenciesController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public InMemoryCacheDependenciesController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult CreateDependentEntries()
        {
            var cts = new CancellationTokenSource();
            _memoryCache.Set(CacheKeys.DependentCTS, cts);
            using (var cacheEntry = _memoryCache.CreateEntry(CacheKeys.Parent))
            {
                cacheEntry.Value = DateTime.Now;
                cacheEntry.RegisterPostEvictionCallback(DependentEvictionCallBack, this);

                _memoryCache.Set(CacheKeys.Child, DateTime.Now, new CancellationChangeToken(cts.Token));
            }
            return RedirectToAction("GetDependentEntries");
        }

        public IActionResult GetDependentEntries()
        {
            return View("dependentCache", new DependentCacheViewModel{
                ParentCachedTime = _memoryCache.Get<DateTime?>(CacheKeys.Parent),
                ChildCachedTime = _memoryCache.Get<DateTime?>(CacheKeys.Child),
                Message = _memoryCache.Get<string>(CacheKeys.DependentMessage)
            });
        }

        public IActionResult RemoveChildEntry()
        {
            _memoryCache.Get<CancellationTokenSource>(CacheKeys.DependentCTS).Cancel();
            return RedirectToAction("GetDependentEntries");
        }

        private static void DependentEvictionCallBack(object key, object value, EvictionReason reason, object state)
        {
            var message = $"Parent Entry was evicted. Reason : {reason}";
            ((InMemoryCacheDependenciesController)state)._memoryCache.Set(CacheKeys.DependentMessage, message);
        }
    }
}