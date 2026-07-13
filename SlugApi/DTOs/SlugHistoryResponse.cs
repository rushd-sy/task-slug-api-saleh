namespace SlugApi.DTOs
{
    public record SlugHistoryResponse(
        int Id,
        string OriginalText,
        string Slug,
        char Separator,
        DateTimeOffset GeneratedAt
    );
}
