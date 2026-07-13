using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SlugApi.Entities;

namespace SlugApi.Configurations
{
    public class SlugRecordConfiguration : IEntityTypeConfiguration<SlugRecord>
    {
        public void Configure(EntityTypeBuilder<SlugRecord> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.OriginalText)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.Slug)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.Separator)
                .IsRequired()
                .HasMaxLength(1);

            builder.Property(x => x.GeneratedAt)
                .IsRequired();
        }
    }
}
