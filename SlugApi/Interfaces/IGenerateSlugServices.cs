using SlugApi.DTOs;

namespace SlugApi.Interfaces
{
    public interface IGenerateSlugServices
    {
        GenerateSlugResponse Generate(GenerateSlugRequest request);

    }
}
