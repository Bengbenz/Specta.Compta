using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;

using Beng.Specta.Compta.Core.DTOs;
using Beng.Specta.Compta.Core.Objects.Auth;

namespace Beng.Specta.Compta.Client.Services.Auth
{
    public class AppAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AppAuthenticationStateProvider> _logger;

        public AppAuthenticationStateProvider(
            HttpClient httpClient,
            ILogger<AppAuthenticationStateProvider> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var account = await _httpClient.GetJsonAsync<UserInfoDTO>("api/account/details");

            var isAuthenticated = (account?.IsAuthenticated).Value;
            var identity = isAuthenticated
                ? new ClaimsIdentity(new[]
                  {
                      new Claim(ClaimTypes.NameIdentifier, account.UserId),
                      new Claim(ClaimTypes.Name, !string.IsNullOrWhiteSpace(account.UserName) ? account.UserName : account.Email),
                      new Claim(ClaimTypes.Email, account.Email),
                      new Claim(ClaimTypes.AuthenticationMethod, account.HasPassword ? "internal" : "external"),
                      new Claim(PermissionConstants.Title, account.Title),
                      new Claim(PermissionConstants.PackedPermissionClaimType, account.PackedPermissions),
                  }, "serverauth")
                : new ClaimsIdentity();
            
            _logger.LogInformation(isAuthenticated ? $"User authentified: {account}" : "Anonymous user");
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
        }

        public void Login()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public void Logout()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));
            NotifyAuthenticationStateChanged(authState);
        }
    }
}