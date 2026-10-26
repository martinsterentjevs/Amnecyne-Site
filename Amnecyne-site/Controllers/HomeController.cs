using System.Diagnostics;
using Amnecyne.Models;
using Amnecyne.Services;
using Microsoft.AspNetCore.Mvc;

namespace Amnecyne.Controllers;

public class HomeController(ProjectsService projectsService) : Controller
{
    // HomeController
public async Task<IActionResult> Index()
{
    var featured = await projectsService.GetFeaturedAsync();
    return View(featured);
}

    public IActionResult AboutMe()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}