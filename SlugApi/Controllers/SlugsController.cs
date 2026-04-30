using Microsoft.AspNetCore.Mvc;
using SlugApi.DTOs;
using SlugApi.Interfaces;
using SlugApi.Services;
using SlugGenerator;
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
            var result = _slugService.Generate(request);
            return Ok(result);
        }

        // test
        [HttpGet]
        [Route("error")]
        public IActionResult Error()
        {
            throw new Exception("test error");
        }

    }
}
