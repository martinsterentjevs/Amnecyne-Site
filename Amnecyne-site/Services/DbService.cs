using Amnecyne.Data;
using Amnecyne.DataModels;
using Amnecyne.Models;
using Microsoft.EntityFrameworkCore;

namespace Amnecyne.Services;

public class DbService(AppDbContext db)
{
    internal Task<List<Project>> GetAllAsync()
    {
        return db.Projects.ToListAsync();
    }
    
    internal Task<Project?> GetBySlugId(string slug)
    {
        return db.Projects.FirstOrDefaultAsync(p => p.Slug == slug);
    }
    /*internal Task<List<ProjectCardViewModel>> GetByTagAsync(string tag)
    {
        return db.Projects
            .Where(p => p.Tags.Contains(tag))
            .OrderByDescending(p => p.UpdatedAt)
            .Select(p => new ProjectCardViewModel ( p.Slug, p.Name, p.Description))
            .ToListAsync();
    }*/
    internal Task<Project?> GetByIdAsync(Guid id)
    {
        return db.Projects
            .Include(p => p.Images.OrderBy(i => i.SortOrder))
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    internal async Task UpdateIconPathAsync(Guid projectId, string? iconPath)
    {
        var project = await db.Projects.FindAsync(projectId);
        if (project is null) return;
        project.IconPath = iconPath;
        project.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
    }

    internal async Task<ProjectImage> AddImageAsync(Guid projectId, string path, string? caption = null)
    {
        var maxSort = await db.ProjectImages
            .Where(i => i.ProjectId == projectId)
            .Select(i => (int?)i.SortOrder)
            .MaxAsync() ?? -1;

        var image = new ProjectImage
        {
            Id = Guid.NewGuid(),
            ProjectId = projectId,
            Path = path,
            Caption = caption,
            SortOrder = maxSort + 1
        };
        db.ProjectImages.Add(image);
        await db.SaveChangesAsync();
        return image;
    }

    internal Task<ProjectImage?> GetImageAsync(Guid imageId)
    {
        return db.ProjectImages.FirstOrDefaultAsync(i => i.Id == imageId);
    }

    internal async Task RemoveImageAsync(Guid imageId)
    {
        var image = await db.ProjectImages.FindAsync(imageId);
        if (image is null) return;
        db.ProjectImages.Remove(image);
        await db.SaveChangesAsync();
    }
    internal Task<List<ProjectCardViewModel>> GetFeaturedAsync()
    {
        return db.Projects
            .Where(p => p.IsFeatured)
            .OrderByDescending(p => p.UpdatedAt)
            .Take(3)
            .Select(p => new ProjectCardViewModel ( p.Slug, p.Name, p.Description))
            .ToListAsync();
    }
    internal Task<List<ChangelogEntry>> GetChangelogAsync(Guid projectId)
    {
        return db.ChangelogEntries
            .Where(c => c.ProjectId == projectId)
            .OrderByDescending(c => c.Date)
            .ToListAsync();
    }
}