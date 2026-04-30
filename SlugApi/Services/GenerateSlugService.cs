using SlugApi.DTOs;
using SlugApi.Interfaces;
using SlugGenerator;

namespace SlugApi.Services
{
    public class GenerateSlugService : IGenerateSlugServices
    {
        public GenerateSlugResponse Generate(GenerateSlugRequest request)
        {
            var separator = request.Separator ?? '-';
            if (string.IsNullOrWhiteSpace(request.Text))
            {
                throw new ArgumentException("Text is required", nameof(request.Text));
            }
                
                var slug = SlugGenerator.SlugGenerator.Generate(request.Text, separator);
                var generatedAt = DateTime.UtcNow;
                return new GenerateSlugResponse(request.Text, slug, generatedAt);
             
        }
    }
}
