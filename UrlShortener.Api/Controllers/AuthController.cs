using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Dtos.Auth.Request;
using UrlShortener.Dtos.Auth.Response;

namespace UrlShortener.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            //LoginResponse result = await _accountService.Login(request);

            //if (string.IsNullOrWhiteSpace(result.JwtToken) || string.IsNullOrWhiteSpace(result.RefreshToken))
            //{
            //    return BadRequest();
            //}

            return Ok();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            //await _accountService.Register(request);

            return Ok();
        }
    }
}