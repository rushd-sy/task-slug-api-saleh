using SlugApi.Entities;

namespace SlugApi.Interfaces
{
    public interface ISlugRepository
    {
        Task SaveAsync(SlugRecord record);
        Task<IEnumerable<SlugRecord>> GetAllAsync();

    }
}
