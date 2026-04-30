namespace SlugApi.DTOs
{
    public record GenerateSlugRequest
    (
        string text ,
        char? separator
        );
}
