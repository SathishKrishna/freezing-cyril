using System;

namespace firstcore.Models
{
    public class DependentCacheViewModel
    {
        public DateTime? ParentCachedTime { get; set; }
        public DateTime? ChildCachedTime { get; set; }
        public string Message { get; set; }
    }
}