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
            GenerateSlugResult result = _slugService.Generate(request);
            Response.Headers["X-Cache"] = result.IsHit ? "HIT" : "MISS";

            return Ok(result);
        }

        [HttpGet("history")]
        public async Task<ActionResult<IEnumerable<GenerateSlugResponse>>> GetSlugsHistory()
        {
            var slugRecords = await _slugService.GetHistoryAsync();
            return Ok(slugRecords);
        }

    }
}
