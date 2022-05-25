using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;
using Beng.Specta.Compta.Core.Dtos;

using Finbuckle.MultiTenant;

namespace Beng.Specta.Compta.Server.Controllers;

public class TenantController : BaseApiController
{
    public TenantController(ILogger<TenantController> logger) : base(logger)
    {
    }

    [HttpGet]
    public ActionResult<string> Info()
    {
        var assembly = typeof(Program).Assembly;

        var creationDate = System.IO.File.GetCreationTime(assembly.Location);
        var version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

        return Ok($"Version: {version}, Last Updated: {creationDate}");
    }

    [HttpGet]
    public ActionResult<TenantInfoDto> CurrentTenant()
    {
        var tenantInfo = HttpContext.GetMultiTenantContext<TenantInfo>()?.TenantInfo;
        TenantInfoDto tenantInfoDto;
        if (tenantInfo != null)
        {
            tenantInfoDto = new TenantInfoDto
            {
                Id = tenantInfo.Id,
                Identifier = tenantInfo.Identifier,
                Name = tenantInfo.Name
            };
        }
        else
        {
            tenantInfoDto = new TenantInfoDto { Name = "Inconnu" };
        }

        Logger.LogDebug($"Tenant found {tenantInfoDto.Name}");
        return Ok(tenantInfoDto);
    }
}