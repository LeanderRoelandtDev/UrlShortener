using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using UrlShortener.Core.Exceptions;
using UrlShortener.Infrastructure.Services;

using UrlShortener.Models;

namespace UrlShortener.Testing.Services.AuthenticationServiceTests
{
    public class GenerateJwtTokenTests
    {
        private readonly IConfiguration config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "$OGGNWy>(5w,hCQGO5!I@_BXe0V{hxSflOYT^l8WF<F",
                ["Jwt:Issuer"] = "UrlShortener.Api",
                ["Jwt:Audience"] = "UrlShortener.Client"
            })
            .Build();

        [Fact]
        public async Task GenerateJWTToken_ValidUser_ReturnsValidToken()
        {
            AuthenticationService service = new AuthenticationService(config);

            User user = new User
            {
                ID = Guid.NewGuid(),
                Email = "Testing@Test.com",
                UserName = "Testing"
            };

            string result = await service.GenerateJWTToken(user);

            Assert.False(string.IsNullOrWhiteSpace(result));

            JwtSecurityTokenHandler handler = new();

            Assert.True(handler.CanReadToken(result));

            JwtSecurityToken token = handler.ReadJwtToken(result);

            Assert.True(token.ValidTo > DateTime.UtcNow);
            Assert.Equal("UrlShortener.Api", token.Issuer);
            Assert.Contains("UrlShortener.Client", token.Audiences);
        }


        [Fact]
        public async Task GenerateJWTToken_NullUser_ThrowsNotFound()
        {
            AuthenticationService service = new AuthenticationService(config);

            ErrorException exception = await Assert.ThrowsAsync<ErrorException>(
                () => service.GenerateJWTToken(null));

            Assert.Equal(HttpStatusCode.NotFound, exception.Code);
        }


        [Fact]
        public async Task GenerateJWTToken_ValidUser_ContainsCorrectUserIdClaim()
        {
            AuthenticationService service = new AuthenticationService(config);

            User user = new User
            {
                ID = Guid.NewGuid(),
                Email = "Testing@Test.com",
                UserName = "Testing"
            };

            string result = await service.GenerateJWTToken(user);

            JwtSecurityTokenHandler handler = new();

            JwtSecurityToken token = handler.ReadJwtToken(result);

            Claim? claim = token.Claims.FirstOrDefault(
                x => x.Type == JwtRegisteredClaimNames.Sub);

            Assert.NotNull(claim);
            Assert.Equal(user.ID.ToString(), claim.Value);
        }


        [Fact]
        public async Task GenerateJWTToken_ValidUser_ContainsCorrectEmailClaim()
        {
            AuthenticationService service = new AuthenticationService(config);

            User user = new User
            {
                ID = Guid.NewGuid(),
                Email = "Testing@Test.com",
                UserName = "Testing"
            };

            string result = await service.GenerateJWTToken(user);

            JwtSecurityTokenHandler handler = new();

            JwtSecurityToken token = handler.ReadJwtToken(result);

            Claim? claim = token.Claims.FirstOrDefault(
                x => x.Type == ClaimTypes.Email);

            Assert.NotNull(claim);
            Assert.Equal(user.Email, claim.Value);
        }
    }
}