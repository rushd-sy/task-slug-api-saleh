using Microsoft.EntityFrameworkCore;
using SlugApi.Date;
using SlugApi.Entities;
using SlugApi.Interfaces;

namespace SlugApi.Repository
{
    public class SlugRepository : ISlugRepository
    {
        private readonly AppDbContext _context;
        public SlugRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<SlugRecord>> GetAllAsync()
        {
            return await _context.slugRecords
                .OrderByDescending(x => x.GeneratedAt)
                .ToListAsync();
        }

        public async Task SaveAsync(SlugRecord slugRecord)
        {
            await _context.slugRecords.AddAsync(slugRecord);
            await _context.SaveChangesAsync();
        }
    }
}
