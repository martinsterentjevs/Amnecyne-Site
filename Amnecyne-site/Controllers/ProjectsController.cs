using Amnecyne.Services;
using Microsoft.AspNetCore.Mvc;

namespace Amnecyne.Controllers;

[Route("projects")]
public class ProjectsController(ProjectsService projectsService) : Controller
{
    [HttpGet("")]
    public IActionResult Index()
    {
        var projects = projectsService.GetAll(); 
        return View(projects); 
    }

    [HttpGet("{slug}")]
    public IActionResult Details(string slug)
    {
        var result = projectsService.GetBySlugId(slug);
         if (result != null)
             return View(slug);
         else 
             return NotFound();
    }

    [HttpGet("{slug}/changelog")]
    public IActionResult Changelog(string slug)
    {
        return View(slug);
    }
}