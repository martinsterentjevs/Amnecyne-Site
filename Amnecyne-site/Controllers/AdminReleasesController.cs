using Amnecyne.Models;
using Amnecyne.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Amnecyne.Controllers;

[Authorize]
[AutoValidateAntiforgeryToken]
[Route("admin/releases")]
public class AdminReleasesController(DbService dbService) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index() => View(await dbService.GetAllReleasesAsync());

    [HttpGet("create")]
    public async Task<IActionResult> Create()
    {
        await PopulateProjectOptions();
        return View(new ReleaseFormViewModel());
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(ReleaseFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            await PopulateProjectOptions();
            return View(vm);
        }
        await dbService.CreateReleaseAsync(vm);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("{id:guid}/edit")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var release = await dbService.GetReleaseByIdAsync(id);
        if (release is null) return NotFound();
        await PopulateProjectOptions();
        return View(ReleaseFormViewModel.FromEntity(release));
    }

    [HttpPost("{id:guid}/edit")]
    public async Task<IActionResult> Edit(Guid id, ReleaseFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            await PopulateProjectOptions();
            return View(vm);
        }
        await dbService.UpdateReleaseAsync(id, vm);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:guid}/delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await dbService.DeleteReleaseAsync(id);
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