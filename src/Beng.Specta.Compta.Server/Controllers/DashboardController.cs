using Microsoft.AspNetCore.Mvc;

namespace Beng.Specta.Compta.Server.Controllers;

public class DashboardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}