using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using SlugApi.DTOs;
using SlugApi.Interfaces;

namespace SlugApi.Services
{
    public class GenerateSlugService : IGenerateSlugServices
    {
        private readonly IMemoryCache _cache;
        private readonly ISlugRepository _slugRepository;
        private const string CacheKeyPrefix = "slug-generator";
        public GenerateSlugService(IMemoryCache cache, ISlugRepository slugRepository)
        {
            _cache = cache;
            _slugRepository = slugRepository;
        }

        public GenerateSlugResult Generate(GenerateSlugRequest request)
        {
            var separator = request.Separator ?? '-';
            var cacheKey = BuildCacheKey(request);
            var generatedAt = DateTime.UtcNow;
            if (_cache.TryGetValue(cacheKey, out GenerateSlugResponse? cached))
            {
                var hitResult = cached! with { GeneratedAt = generatedAt };
                return new GenerateSlugResult(hitResult!, IsHit: true);
            }

            var slug = SlugGenerator.SlugGenerator.Generate(request.Text, separator);
            var result = new GenerateSlugResponse(request.Text, slug, generatedAt);

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                .SetAbsoluteExpiration(TimeSpan.FromHours(1))
                .SetPriority(CacheItemPriority.Normal)
                .SetSize(1);

            _cache.Set(cacheKey, result, cacheOptions);

            return new GenerateSlugResult(result, IsHit: false);
        }

        public static string BuildCacheKey(GenerateSlugRequest request)
        {
            var separator = request.Separator ?? '-';
            string key = $"{separator}{request.Text}";

            using var sha = SHA256.Create();
            var hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(key));
            var hash = Convert.ToHexString(hashBytes);

            return $"{CacheKeyPrefix}:{hash}";
        }

        public async Task<IEnumerable<GenerateSlugResponse>> GetHistoryAsync()
        {
            var slugsHistory = await _slugRepository.GetAllAsync();
            return slugsHistory.Select(r => new GenerateSlugResponse(
                r.OriginalText,
                r.Slug,
                r.GeneratedAt
            ));
        }

    }
}
