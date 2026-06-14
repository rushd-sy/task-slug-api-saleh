namespace SlugApi.DTOs
{
    public record GenerateSlugResult
    (
        GenerateSlugResponse Response,
        bool IsHit
    );
}
