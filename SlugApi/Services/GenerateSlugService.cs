using Microsoft.Extensions.Caching.Memory;
using SlugApi.DTOs;
using SlugApi.Interfaces;

namespace SlugApi.Services
{
    public class GenerateSlugService : IGenerateSlugServices
    {
        private readonly IMemoryCache _cache;

        public GenerateSlugService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public (GenerateSlugResponse Response, bool IsHit)
            Generate(GenerateSlugRequest request)
        {
            var separator = request.Separator ?? '-';
            var cacheKey = $"{request.Text}:{separator}";

            if (_cache.TryGetValue(cacheKey, out GenerateSlugResponse? cached))
                return (cached!, true);


            var slug = SlugGenerator.SlugGenerator.Generate(request.Text, separator);
            var result = new GenerateSlugResponse(request.Text, slug, DateTime.UtcNow);

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                .SetAbsoluteExpiration(TimeSpan.FromHours(1))
                .SetPriority(CacheItemPriority.Normal);

            _cache.Set(cacheKey, result, cacheOptions);

            return (result, false);
        }
    }
}
