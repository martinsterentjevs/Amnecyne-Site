using Microsoft.AspNetCore.Mvc;

namespace Amnecyne.Controllers;

public class AdminController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}