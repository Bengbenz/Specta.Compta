using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Beng.Specta.Compta.Server.Controllers
{
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
}
