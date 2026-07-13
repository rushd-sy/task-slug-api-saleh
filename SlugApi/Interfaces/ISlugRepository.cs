using SlugApi.DTOs;
using SlugApi.Entities;

namespace SlugApi.Interfaces
{
    public interface ISlugRepository
    {
        Task SaveAsync(SlugRecord record);
        Task<PagedResult<SlugRecord>> GetAllAsync(int Page, int PageSize);

    }
}
