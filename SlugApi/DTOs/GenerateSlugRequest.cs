namespace SlugApi.DTOs
{
    public record GenerateSlugRequest
    (
        string Text ,
        char? Separator
        );
}
