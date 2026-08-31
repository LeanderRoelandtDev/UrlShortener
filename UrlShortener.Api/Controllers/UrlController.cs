using Microsoft.AspNetCore.Mvc;
using UrlShortener.Core.Interfaces.Services;

namespace UrlShortener.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlController(IUrlService urlService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Save()
        {
            urlService.Save("Test");
            
            return Ok();
        }
    }
}