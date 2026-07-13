namespace SlugApi.Entities
{
    public class SlugRecord
    {
        public int Id { get; set; }
        public string OriginalText { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public char Separator { get; set; }
        public DateTimeOffset GeneratedAt { get; set; }

    }
}
