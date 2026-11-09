using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace UrlShortener.Client.Handlers
{
    public class ApiAuthStateHandler : AuthenticationStateProvider
    {
        private readonly ApiHandler ApiHandler;

        public ApiAuthStateHandler(ApiHandler apiHandler)
        {
            ApiHandler = apiHandler;
            ApiHandler.Unauthorized += () => _ = NotifyAuthChanged();
        }

        private static readonly AuthenticationState Anonymous = new(new ClaimsPrincipal(new ClaimsIdentity()));

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var result = await ApiHandler.Send<List<ClaimDto>>(HttpMethod.Get, "auth/me");


            if (!result.Success || result.Data is null || result.Data.Count == 0)
                return Anonymous;

            ClaimsIdentity identity = new ClaimsIdentity(result.Data.Select(c => new Claim(c.Type, c.Value)), "cookie");

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }


        public async Task NotifyAuthChanged()
        {
            var task = GetAuthenticationStateAsync();

            NotifyAuthenticationStateChanged(task);

            await task;
        }
    }
}


public record ClaimDto(string Type, string Value);