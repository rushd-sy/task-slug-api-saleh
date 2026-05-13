using SlugApi.DTOs;
using SlugApi.Interfaces;

namespace SlugApi.Services
{
    public class GenerateSlugService : IGenerateSlugServices
    {
        public GenerateSlugResponse Generate(GenerateSlugRequest request)
        {
            var separator = request.Separator ?? '-';

            var slug = SlugGenerator.SlugGenerator.Generate(request.Text, separator);
            var generatedAt = DateTime.UtcNow;
            return new GenerateSlugResponse(request.Text, slug, generatedAt);

        }
    }
}
