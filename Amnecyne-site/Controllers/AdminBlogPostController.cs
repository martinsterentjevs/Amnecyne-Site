using Amnecyne.Models;
using Amnecyne.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Amnecyne.Controllers;

[Authorize]
[AutoValidateAntiforgeryToken]
[Route("admin/blog")]
public class AdminBlogPostsController(DbService dbService) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index() => View(await dbService.GetAllBlogPostsAsync());

    [HttpGet("create")]
    public async Task<IActionResult> Create()
    {
        await PopulateProjectOptions();
        return View(new BlogPostFormViewModel());
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(BlogPostFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            await PopulateProjectOptions();
            return View(vm);
        }
        await dbService.CreateBlogPostAsync(vm);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("{id:guid}/edit")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var post = await dbService.GetBlogPostByIdAsync(id);
        if (post is null) return NotFound();
        await PopulateProjectOptions();
        return View(BlogPostFormViewModel.FromEntity(post));
    }

    [HttpPost("{id:guid}/edit")]
    public async Task<IActionResult> Edit(Guid id, BlogPostFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            await PopulateProjectOptions();
            return View(vm);
        }
        await dbService.UpdateBlogPostAsync(id, vm);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:guid}/delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await dbService.DeleteBlogPostAsync(id);
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateProjectOptions()
    {
        var projects = await dbService.GetAllAsync();
        ViewData["EntityReferenceOptions"] = projects
            .Select(p => new SelectListItem(p.Name, p.Id.ToString()))
            .ToList();
    }
}