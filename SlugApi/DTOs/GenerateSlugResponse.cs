namespace SlugApi.DTOs
{
    public record GenerateSlugResponse
    (
        string OriginalText,
        string Slug,
        DateTimeOffset GeneratedAt
        );
}
