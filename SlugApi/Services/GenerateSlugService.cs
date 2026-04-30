using SlugApi.DTOs;
using SlugApi.Interfaces;
using SlugGenerator;

namespace SlugApi.Services
{
    public class GenerateSlugService : IGenerateSlugServices
    {
        public GenerateSlugResponse Generate(GenerateSlugRequest request)
        {
            var separator = request.separator ?? '-';
            var slug = SlugGenerator.SlugGenerator.Generate(request.text,separator);
            var generatedAt = DateTime.Now;

            return new GenerateSlugResponse(request.text, slug, generatedAt);
        }
    }
}
