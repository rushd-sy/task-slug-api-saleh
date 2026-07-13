using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using SlugApi.DTOs;
using SlugApi.Entities;
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

        public async Task<GenerateSlugResult> GenerateAsync(GenerateSlugRequest request)
        {
            var separator = request.Separator ?? '-';
            var cacheKey = BuildCacheKey(request);
            var generatedAt = DateTime.UtcNow;

            if (_cache.TryGetValue(cacheKey, out GenerateSlugResponse? cached))
            {
                await _slugRepository.SaveAsync(new SlugRecord
                {
                    OriginalText = request.Text,
                    Slug = cached!.Slug,
                    Separator = separator,
                    GeneratedAt = generatedAt
                });

                return new GenerateSlugResult(cached.OriginalText, cached.Slug, generatedAt, IsHit: true);
            }

            var slug = SlugGenerator.SlugGenerator.Generate(request.Text, separator);
            var result = new GenerateSlugResponse(request.Text, slug, generatedAt);

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                .SetAbsoluteExpiration(TimeSpan.FromHours(1))
                .SetPriority(CacheItemPriority.Normal)
                .SetSize(1);

            _cache.Set(cacheKey, result, cacheOptions);

            await _slugRepository.SaveAsync(new SlugRecord
            {
                OriginalText = request.Text,
                Slug = slug,
                Separator = separator,
                GeneratedAt = generatedAt
            });

            return new GenerateSlugResult(result.OriginalText, result.Slug, result.GeneratedAt, IsHit: false);
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

        public async Task<PaginatedResponse<SlugHistoryResponse>> GetHistoryAsync(int page, int pageSize)
        {
            var result = await _slugRepository.GetAllAsync(page, pageSize);

            var items = result.Items.Select(r => new SlugHistoryResponse(
                r.Id,
                r.OriginalText,
                r.Slug,
                r.Separator,
                r.GeneratedAt
            ));

            return new PaginatedResponse<SlugHistoryResponse>(
                items,
                page,
                pageSize,
                result.TotalCount,
                (int)Math.Ceiling((double)result.TotalCount / pageSize)
            );
        }

    }
}
