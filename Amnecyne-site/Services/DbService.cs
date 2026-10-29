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
            .Include(p => p.Tags)
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
    internal Task<List<Tag>> GetAllTagsAsync()
    {
        return db.Tags.ToListAsync();
    }

    internal async Task<Project> CreateProjectAsync(ProjectFormViewModel vm)
    {
        var tags = await db.Tags.Where(t => vm.SelectedTagIds.Contains(t.Id)).ToListAsync();
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = vm.Name,
            Description = vm.Description,
            Slug = vm.Slug,
            PrimaryColor = vm.PrimaryColor,
            State = vm.State,
            GithubVisibility = vm.GithubVisibility,
            GithubLink = vm.GithubLink,
            LatestRelease = vm.LatestRelease,
            IsFeatured = vm.IsFeatured,
            Tags = tags,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        db.Projects.Add(project);
        await db.SaveChangesAsync();
        return project;
    }

    internal async Task UpdateProjectAsync(Guid id, ProjectFormViewModel vm)
    {
        var project = await db.Projects.Include(p => p.Tags).FirstOrDefaultAsync(p => p.Id == id);
        if (project is null) return;

        project.Name = vm.Name;
        project.Description = vm.Description;
        project.Slug = vm.Slug;
        project.PrimaryColor = vm.PrimaryColor;
        project.State = vm.State;
        project.GithubVisibility = vm.GithubVisibility;
        project.GithubLink = vm.GithubLink;
        project.LatestRelease = vm.LatestRelease;
        project.IsFeatured = vm.IsFeatured;
        project.UpdatedAt = DateTime.UtcNow;

        var tags = await db.Tags.Where(t => vm.SelectedTagIds.Contains(t.Id)).ToListAsync();
        project.Tags.Clear();
        foreach (var tag in tags) project.Tags.Add(tag);

        await db.SaveChangesAsync();
    }

    internal async Task DeleteProjectAsync(Guid id)
    {
        var project = await db.Projects.FindAsync(id);
        if (project is null) return;
        db.Projects.Remove(project);
        await db.SaveChangesAsync();
    }
}