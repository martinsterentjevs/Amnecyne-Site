using Amnecyne.Models;
using Amnecyne.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Amnecyne.Controllers;

[Authorize]
[AutoValidateAntiforgeryToken]
[Route("admin/changelog")]
public class AdminChangelogController(DbService dbService) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index() => View(await dbService.GetAllChangelogEntriesAsync());

    [HttpGet("create")]
    public async Task<IActionResult> Create()
    {
        await PopulateProjectOptions();
        return View(new ChangelogFormViewModel());
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(ChangelogFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            await PopulateProjectOptions();
            return View(vm);
        }
        await dbService.CreateChangelogEntryAsync(vm);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("{id:guid}/edit")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var entry = await dbService.GetChangelogEntryByIdAsync(id);
        if (entry is null) return NotFound();
        await PopulateProjectOptions();
        return View(ChangelogFormViewModel.FromEntity(entry));
    }

    [HttpPost("{id:guid}/edit")]
    public async Task<IActionResult> Edit(Guid id, ChangelogFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            await PopulateProjectOptions();
            return View(vm);
        }
        await dbService.UpdateChangelogEntryAsync(id, vm);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:guid}/delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await dbService.DeleteChangelogEntryAsync(id);
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