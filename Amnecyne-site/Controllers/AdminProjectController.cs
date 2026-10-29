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

    [HttpGet("{id:guid}/edit")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var project = await dbService.GetByIdAsync(id);
        if (project is null) return NotFound();
        return View(project);
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