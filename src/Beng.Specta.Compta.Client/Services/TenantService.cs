using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Beng.Specta.Compta.Core.Dtos;

namespace Beng.Specta.Compta.Client.Services;

public class TenantService
{
    private readonly NavigationManager _navigationManager;
    private readonly HttpClient _httpClient;
    private readonly ILogger<TenantService> _logger;

    private TenantInfoDto? _tenantInfo;

    public TenantService (
        NavigationManager navigationManager,
        HttpClient httpClient,
        ILogger<TenantService> logger)
    {
        _navigationManager = navigationManager;
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<TenantInfoDto?> GetTenantInfoAsync()
    {
        if (_tenantInfo is null)
        {
            _tenantInfo = await _httpClient.GetFromJsonAsync<TenantInfoDto>("api/tenant/currenttenant");
        }

        return _tenantInfo;
    }
}