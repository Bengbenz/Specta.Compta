using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Beng.Specta.Compta.Core.Dtos.Identities;
using Beng.Specta.Compta.Core.Objects.Identities;

namespace Beng.Specta.Compta.Client.Services.Identities;

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
        var account = await _httpClient.GetFromJsonAsync<UserInfoDto>("api/account/details");

        var isAuthenticated = account!.IsAuthenticated;
        var identity = isAuthenticated
            ? new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, account.UserId),
                new Claim(ClaimTypes.Name, !string.IsNullOrWhiteSpace(account.UserName) ? account.UserName : account.Email),
                new Claim(ClaimTypes.Email, account.Email),
                new Claim(ClaimTypes.AuthenticationMethod, account.HasPassword ? "internal" : "external"),
                new Claim(PermissionConstants.Title, account.Title)
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