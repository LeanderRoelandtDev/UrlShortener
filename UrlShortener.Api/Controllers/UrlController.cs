using Microsoft.AspNetCore.Mvc;
using UrlShortener.Core.Interfaces.Services;
using UrlShortener.Dtos.Url.Request;

namespace UrlShortener.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlController(IUrlService urlService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] CreateShortUrlRequest request)
        {
            await urlService.Save(request);
            
            return Ok();
        }
    }
}