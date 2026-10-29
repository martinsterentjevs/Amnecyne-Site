using Amnecyne.Models;
using Amnecyne.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Amnecyne.Controllers;

[Authorize]
[Route("admin/projects")]
public class AdminProjectsController(DbService dbService, IFileStorageService fileStorage) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var projects = await dbService.GetAllAsync();
        return View(projects);
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create()
    {
        ViewData["AllTags"] = await dbService.GetAllTagsAsync();
        return View(new ProjectFormViewModel());
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(ProjectFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            ViewData["AllTags"] = await dbService.GetAllTagsAsync();
            return View(vm);
        }
        var project = await dbService.CreateProjectAsync(vm);
        return RedirectToAction(nameof(Edit), new { id = project.Id });
    }

    [HttpGet("{id:guid}/edit")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var project = await dbService.GetByIdAsync(id);
        if (project is null) return NotFound();
        ViewData["AllTags"] = await dbService.GetAllTagsAsync();
        return View(ProjectFormViewModel.FromEntity(project));
    }

    [HttpPost("{id:guid}/edit")]
    public async Task<IActionResult> Edit(Guid id, ProjectFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            ViewData["AllTags"] = await dbService.GetAllTagsAsync();
            return View(vm);
        }
        await dbService.UpdateProjectAsync(id, vm);
        return RedirectToAction(nameof(Edit), new { id });
    }

    [HttpPost("{id:guid}/delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var project = await dbService.GetByIdAsync(id); // includes Images
        if (project is null) return NotFound();

        if (!string.IsNullOrEmpty(project.IconPath))
            fileStorage.Delete(project.IconPath);
        foreach (var img in project.Images)
            fileStorage.Delete(img.Path);

        await dbService.DeleteProjectAsync(id);
        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost("{id:guid}/icon")]
    public async Task<IActionResult> UploadIcon(Guid id, IFormFile file, CancellationToken ct)
    {
        var project = await dbService.GetByIdAsync(id);
        if (project is null) return NotFound();

        var oldPath = project.IconPath;
        var relativePath = await fileStorage.SaveAsync(id, "icon", "icon", file, ct);
        await dbService.UpdateIconPathAsync(id, relativePath);

        if (!string.IsNullOrEmpty(oldPath))
            fileStorage.Delete(oldPath);

        return RedirectToAction(nameof(Edit), new { id });
    }

    [HttpPost("{id:guid}/images")]
    public async Task<IActionResult> UploadShowcaseImage(Guid id, IFormFile file, CancellationToken ct)
    {
        var imageId = Guid.NewGuid();
        var relativePath = await fileStorage.SaveAsync(id, "showcase", imageId.ToString(), file, ct);
        await dbService.AddImageAsync(id, relativePath);
        return RedirectToAction(nameof(Edit), new { id });
    }

    [HttpPost("{projectId:guid}/images/{imageId:guid}/delete")]
    public async Task<IActionResult> DeleteShowcaseImage(Guid imageId, Guid projectId)
    {
        var image = await dbService.GetImageAsync(imageId);
        if (image is not null)
        {
            fileStorage.Delete(image.Path);
            await dbService.RemoveImageAsync(imageId);
        }
        return RedirectToAction(nameof(Edit), new { id = projectId });
    }
}