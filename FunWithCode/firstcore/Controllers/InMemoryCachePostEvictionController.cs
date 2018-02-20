using System;
using firstcore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace firstcore.Controllers
{
    public class InMemoryCachePostEvictionController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public InMemoryCachePostEvictionController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public IActionResult CreateCallBackEntry()
        {
            var callbackEntryOptions = new MemoryCacheEntryOptions()
            .SetPriority(CacheItemPriority.Low)
            .SetAbsoluteExpiration(TimeSpan.FromSeconds(3))
            .RegisterPostEvictionCallback(callback:EvictionCallback, state: this);
            _memoryCache.Set(CacheKeys.CallbackEntry, DateTime.Now, callbackEntryOptions);
            return RedirectToAction("GetCallBackEntry");
        }

        public IActionResult GetCallBackEntry()
        {
            return View("CallBack", new CallBackViewModel{
                CacheTime = _memoryCache.Get<DateTime?>(CacheKeys.CallbackEntry),
                Message = _memoryCache.Get<string>(CacheKeys.CallbackMessage)
            });
        }

        public IActionResult RemoveCallBackEntry()
        {
            _memoryCache.Remove(CacheKeys.CallbackEntry);
            return RedirectToAction("GetCallBackEntry");
        }

        private static void EvictionCallback(object key, object value, EvictionReason reason, object state)
        {
            var message = $"Entry was evicted. Reason : {reason}.";
            ((InMemoryCachePostEvictionController)state)._memoryCache.Set(CacheKeys.CallbackMessage, message);
        }
    }
}