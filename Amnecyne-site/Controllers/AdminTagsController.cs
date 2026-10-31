using Amnecyne.Models;
using Amnecyne.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Amnecyne.Controllers;

[Authorize]
[AutoValidateAntiforgeryToken]
[Route("admin/tags")]
public class AdminTagsController(DbService dbService) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index() => View(await dbService.GetAllTagsAsync());

    [HttpGet("create")]
    public IActionResult Create() => View(new TagFormViewModel());

    [HttpPost("create")]
    public async Task<IActionResult> Create(TagFormViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);
        await dbService.CreateTagAsync(vm);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("{id:guid}/edit")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var tag = await dbService.GetTagByIdAsync(id);
        if (tag is null) return NotFound();
        return View(TagFormViewModel.FromEntity(tag));
    }

    [HttpPost("{id:guid}/edit")]
    public async Task<IActionResult> Edit(Guid id, TagFormViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);
        await dbService.UpdateTagAsync(id, vm);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:guid}/delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await dbService.DeleteTagAsync(id);
        return RedirectToAction(nameof(Index));
    }
}