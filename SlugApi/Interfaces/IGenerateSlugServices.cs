using SlugApi.DTOs;

namespace SlugApi.Interfaces
{
    public interface IGenerateSlugServices
    {
        (GenerateSlugResponse Response, bool IsHit) Generate(GenerateSlugRequest request);

    }
}
