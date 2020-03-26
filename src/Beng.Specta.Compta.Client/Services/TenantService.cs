using System.Linq;
using Beng.Specta.Compta.Core.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace Beng.Specta.Compta.Client.Services {
    public class TenantService
    {
        private NavigationManager NavigationManager { get; }

        protected ILogger<TenantService> Logger { get; set; }

        public TenantInfoDTO TenantInfo { get; set; }

        public TenantService (ILogger<TenantService> logger, NavigationManager navigationManager)
        {
            NavigationManager = navigationManager;
            Logger = logger;
        }

        public string GetTenantIdentifier()
        {
            string currentUrlWithoutBase = NavigationManager.Uri.Substring(NavigationManager.BaseUri.Length);
            var separator = "/";
            var tenantIdentifier = currentUrlWithoutBase.Split(separator).FirstOrDefault();
            Logger.LogDebug($"{GetType().Name}: Current uri={currentUrlWithoutBase} from complete Url={NavigationManager.Uri}, BaseUri={NavigationManager.BaseUri}, TenantId requested={tenantIdentifier}");
            return tenantIdentifier;
        }
    }
}