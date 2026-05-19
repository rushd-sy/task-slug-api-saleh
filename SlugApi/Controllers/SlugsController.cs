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
        public IActionResult Generate(GenerateSlugRequest request)
        {
            var (result, isHit) = _slugService.Generate(request);
            Response.Headers["X-Cache"] = isHit ? "HIT" : "MISS";
            return Ok(result);
        }

    }
}
