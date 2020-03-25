using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Beng.Specta.Compta.Server.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseApiController : Controller
    {
        protected ILogger Logger { get; set; }

        protected BaseApiController(ILogger logger)
        {
            Logger = logger;
        }
    }
}
