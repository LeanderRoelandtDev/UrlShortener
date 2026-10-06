using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrlShortener.Core.Exceptions;
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


        [Authorize]
        [HttpPost("CreateShortUrl")]
        public async Task<IActionResult> Create([FromBody] CreateShortUrlRequest request)
        {
            Guid userId = GetUserId();

            string shortUrl = await urlService.Create(request, userId);
            
            return Ok(shortUrl);
        }


        [Authorize]
        [HttpPut("EditShortUrl")]
        public async Task<IActionResult> Edit([FromBody] EditUrlEntityRequest request)
        {
            Guid userId = GetUserId();

            await urlService.Edit(request, userId);

            return NoContent();
        }



        private Guid GetUserId()
        {
            string userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                                    ?? User.FindFirstValue("sub")
                                    ?? throw ErrorException.BadRequest("User ID claim is missing.");

            if (!Guid.TryParse(userIdClaim, out Guid userId))
                throw ErrorException.BadRequest("User ID claim is not a valid GUID.");

            return userId;
        }
    }
}