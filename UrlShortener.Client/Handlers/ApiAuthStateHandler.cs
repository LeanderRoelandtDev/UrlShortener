using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace UrlShortener.Client.Handlers
{
    public class ApiAuthStateHandler(ApiHandler apiHandler) : AuthenticationStateProvider
    {
        private static readonly AuthenticationState Anonymous = new(new ClaimsPrincipal(new ClaimsIdentity()));

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var result = await apiHandler.Send<List<ClaimDto>>(HttpMethod.Get, "auth/me");


            if (!result.Success || result.Data is null || result.Data.Count == 0)
                return Anonymous;

            ClaimsIdentity identity = new ClaimsIdentity(result.Data.Select(c => new Claim(c.Type, c.Value)), "cookie");

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }


        public void NotifyAuthChanged() => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}


public record ClaimDto(string Type, string Value);