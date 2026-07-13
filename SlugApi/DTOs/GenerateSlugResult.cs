namespace SlugApi.DTOs
{
    public record GenerateSlugResult
    (
    string OriginalText,
    string Slug,
    DateTimeOffset GeneratedAt,
    bool IsHit
);
}
