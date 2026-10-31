using Amnecyne.Services;
using Microsoft.AspNetCore.Mvc;

namespace Amnecyne.Controllers;

[Route("projects")]
public class ProjectsController(ProjectsService projectsService) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(string? tag)
    {
        var projects = /*string.IsNullOrEmpty(tag)
            ? await projectsService.GetAllAsync()
            : await projectsService.GetByTagAsync(tag);*/ await projectsService.GetAllAsync();
        return View(projects);
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> Details(string slug)
    {
        var result = await projectsService.GetBySlugId(slug);
        if (result != null)
            return View(result);   // was: View(slug)
        return NotFound();
    }
    [HttpGet("{slug}/changelog")]
    public async Task<IActionResult> Changelog(string slug)
    {
        var project = await projectsService.GetBySlugId(slug);
        if (project == null) return NotFound();

        var entries = await projectsService.GetChangelogAsync(project.Id);
        return View((project, entries));
    }
    [HttpGet("{slug}/blog/{postSlug}")]
    public async Task<IActionResult> BlogPost(string slug, string postSlug)
    {
        var post = await projectsService.GetBlogPostBySlugAsync(slug, postSlug);
        if (post == null) return NotFound();
        return View(post);
    }
}