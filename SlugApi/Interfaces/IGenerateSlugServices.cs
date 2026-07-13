using SlugApi.DTOs;

namespace SlugApi.Interfaces
{
    public interface IGenerateSlugServices
    {
        Task<GenerateSlugResult> GenerateAsync(GenerateSlugRequest request);
        Task<PaginatedResponse<SlugHistoryResponse>> GetHistoryAsync(int Page, int PageSize);
    }
}
