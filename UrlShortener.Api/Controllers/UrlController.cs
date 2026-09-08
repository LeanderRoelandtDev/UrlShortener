using Microsoft.AspNetCore.Mvc;
using UrlShortener.Core.Interfaces.Services;
using UrlShortener.Dtos.Url.Request;

namespace UrlShortener.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlController(IUrlService urlService) : ControllerBase
    {
        [HttpPost("GetOriginalUrl")]
        public async Task<IActionResult> GetOriginalUrl([FromBody] GetOriginalUrlRequest request)
        {
            string originalUrl = await urlService.GetOriginalUrl(request.ShortUrl);

            return Ok(originalUrl);
        }

        [HttpPost("CreateShortUrl")]
        public async Task<IActionResult> Create([FromBody] CreateShortUrlRequest request)
        {
            string shortUrl = await urlService.Create(request);
            
            return Ok(shortUrl);
        }
    }
}