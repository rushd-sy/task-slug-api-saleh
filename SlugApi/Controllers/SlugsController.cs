using Microsoft.AspNetCore.Mvc;
using SlugApi.DTOs;
using SlugApi.Interfaces;
namespace SlugApi.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class SlugsController : ControllerBase
    {
        private readonly IGenerateSlugServices _slugService;

        public SlugsController(IGenerateSlugServices slugService)
        {
            _slugService = slugService;
        }

        [HttpPost]
        public async Task<ActionResult<GenerateSlugResponse>> Generate(GenerateSlugRequest request)
        {
            var result = await _slugService.GenerateAsync(request);
            Response.Headers["X-Cache"] = result.IsHit ? "HIT" : "MISS";

            return Ok(result);
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetSlugsHistory(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _slugService.GetHistoryAsync(page, pageSize);
            return Ok(result);
        }

    }
}
