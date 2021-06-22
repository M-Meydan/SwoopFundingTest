using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters
{
    public class CacheResourceFilter : IResourceFilter
    {
        ITestableCache _cache;
        string _cacheKey;
        public CacheResourceFilter() { }

        public CacheResourceFilter(ITestableCache cache) { _cache = cache; }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            _cacheKey = context.HttpContext.Request.Path.ToString();

            if(_cache.TryGet(_cacheKey,out object cachedResult))
                context.Result = new ObjectResult(cachedResult);
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            if (context.Exception != null) return;

            if (!string.IsNullOrEmpty(_cacheKey) && !_cache.TryGet(_cacheKey, out object cachedResult) && cachedResult!=null)
            {
                var result = context.Result as ObjectResult;
                if (result != null) _cache.Set(_cacheKey, (context.Result as ObjectResult).Value);
            }
        }
    }
}
