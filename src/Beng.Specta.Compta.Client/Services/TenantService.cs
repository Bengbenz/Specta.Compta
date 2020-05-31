using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

using Beng.Specta.Compta.Core.DTOs;

namespace Beng.Specta.Compta.Client.Services
{
    public class TenantService
    {
        private readonly NavigationManager _navigationManager;
        private readonly HttpClient _httpClient;
        private readonly ILogger<TenantService> _logger;

        private TenantInfoDTO _tenantInfo;

        public TenantService (
            NavigationManager navigationManager,
            HttpClient httpClient,
            ILogger<TenantService> logger)
        {
            _navigationManager = navigationManager;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<TenantInfoDTO> GetTenantInfoAsync()
        {
            if (_tenantInfo == null)
            {
                _tenantInfo = await _httpClient.GetJsonAsync<TenantInfoDTO>("api/tenant/currenttenant");
            }

            return _tenantInfo;
        }
    }
}