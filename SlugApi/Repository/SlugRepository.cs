using Microsoft.EntityFrameworkCore;
using SlugApi.Date;
using SlugApi.DTOs;
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

        public async Task<PagedResult<SlugRecord>> GetAllAsync(int page, int pageSize)
        {
            var query = _context.SlugRecords.OrderByDescending(x => x.GeneratedAt);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new PagedResult<SlugRecord>(items, totalCount);

        }

        public async Task SaveAsync(SlugRecord slugRecord)
        {
            await _context.SlugRecords.AddAsync(slugRecord);
            await _context.SaveChangesAsync();
        }
    }
}
