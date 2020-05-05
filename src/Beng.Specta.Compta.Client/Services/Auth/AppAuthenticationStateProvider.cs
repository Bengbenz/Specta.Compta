using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

using Beng.Specta.Compta.Core.DTOs;
using System;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Beng.Specta.Compta.Client.Services.Auth
{
    public class AppAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient HttpClient;
        private readonly ILogger<AppAuthenticationStateProvider> Logger;

        public AppAuthenticationStateProvider(HttpClient httpClient, ILogger<AppAuthenticationStateProvider> logger)
        {
            HttpClient = httpClient;
            Logger = logger;
        }
        
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var userInfo = new UserInfoDTO();
            try
            {
                userInfo = await HttpClient.GetJsonAsync<UserInfoDTO>("/user");
                Logger.LogDebug($"{GetType().Name} GetAutentification State : user : {userInfo.UserName} ");
            }
            catch(JsonException ex)
            {
                Logger.LogError($"{GetType().Name} Can't get user info.\n {ex}");
            }
            // var userInfo = new UserInfoDTO { UserName = "Tiaani", IsAuthenticated = true };
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, userInfo.UserName),
                // new Claim(PermissionConstants.PackedPermissionClaimType, userInfo.PackedPermissions),
            };
            
            var identity = userInfo.IsAuthenticated
                ? new ClaimsIdentity(claims, "serverauth")
                : new ClaimsIdentity();
                
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
        }
    }
}