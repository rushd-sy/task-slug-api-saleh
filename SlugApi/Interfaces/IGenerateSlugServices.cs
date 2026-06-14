using SlugApi.DTOs;

namespace SlugApi.Interfaces
{
    public interface IGenerateSlugServices
    {
        GenerateSlugResult Generate(GenerateSlugRequest request);

    }
}
