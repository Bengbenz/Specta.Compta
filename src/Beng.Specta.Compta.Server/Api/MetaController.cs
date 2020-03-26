using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Beng.Specta.Compta.Core.DTOs;

using Finbuckle.MultiTenant;
using System.Threading.Tasks;

namespace Beng.Specta.Compta.Server.Api
{
    public class MetaController : BaseApiController
    {
        public MetaController(ILogger<MetaController> logger) : base(logger)
        {
        }

        [HttpGet("/info")]
        public ActionResult<string> Info()
        {
            var assembly = typeof(Startup).Assembly;

            var creationDate = System.IO.File.GetCreationTime(assembly.Location);
            var version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

            return Ok($"Version: {version}, Last Updated: {creationDate}");
        }

        [HttpGet("/tenant")]
        public TenantInfoDTO GetTenant()
        {
            TenantInfo tenantInfo = HttpContext.GetMultiTenantContext()?.TenantInfo;
            TenantInfoDTO tenantInfoDto;
            if(tenantInfo != null)
            {
                tenantInfoDto = new TenantInfoDTO()
                {
                    Id = tenantInfo.Id,
                    Identifier = tenantInfo.Identifier,
                    Name = tenantInfo.Name
                };
            }
            else
            {
                tenantInfoDto = new TenantInfoDTO { Name = "Inconnu" };
            }

            Logger.LogDebug($"Tenant found {tenantInfoDto?.Name}");
            return tenantInfoDto;
        }

        [HttpGet("/tenantname")]
        public string GetTenantName()
        {
            TenantInfo tenantInfo = HttpContext.GetMultiTenantContext()?.TenantInfo;
            Logger.LogDebug($"Tenant found {tenantInfo?.Name}");
            if(tenantInfo != null)
            {
                return tenantInfo.Name;
            }

            return "Inconnu";
        }
    }
}
