using Microsoft.EntityFrameworkCore;
using SlugApi.Entities;

namespace SlugApi.Date
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
        public DbSet<SlugRecord> slugRecords => Set<SlugRecord>();

    }
}
