using Microsoft.AspNetCore.Mvc;

namespace Beng.Specta.Compta.Server.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public abstract class BaseApiController : Controller
{
    protected ILogger Logger { get; }

    protected BaseApiController(ILogger logger)
    {
        Logger = logger;
    }
}