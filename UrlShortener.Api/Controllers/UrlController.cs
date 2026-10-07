using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
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
            string shortUrl = await urlService.Create(request, GetUserId());
            
            return Ok(shortUrl);
        }


        [Authorize]
        [HttpPut("EditShortUrl")]
        public async Task<IActionResult> Edit([FromBody] EditUrlEntityRequest request)
        {
            await urlService.Edit(request, GetUserId());

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{shortUrl}")]
        public async Task<IActionResult> Delete([RegularExpression("^[A-Za-z0-9]{8}$")] string shortUrl)
        {
            await urlService.Delete(shortUrl, GetUserId());

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