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
            .Include(p => p.Releases.OrderByDescending(r => r.ReleaseDate))
            .Include(p => p.BlogPosts.OrderByDescending(b => b.PublishedAt))
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
    internal Task<List<ProjectRelease>> GetAllReleasesAsync()
{
    return db.ProjectReleases.Include(r => r.Project).OrderByDescending(r => r.ReleaseDate).ToListAsync();
}

internal Task<ProjectRelease?> GetReleaseByIdAsync(Guid id)
{
    return db.ProjectReleases.FirstOrDefaultAsync(r => r.Id == id);
}

internal async Task<ProjectRelease> CreateReleaseAsync(ReleaseFormViewModel vm)
{
    var release = new ProjectRelease
    {
        Id = Guid.NewGuid(),
        ProjectId = vm.ProjectId,
        Version = vm.Version,
        ReleaseDate = vm.ReleaseDate,
        DownloadUrl = vm.DownloadUrl,
        Notes = vm.Notes
    };
    db.ProjectReleases.Add(release);
    await db.SaveChangesAsync();
    return release;
}

internal async Task UpdateReleaseAsync(Guid id, ReleaseFormViewModel vm)
{
    var release = await db.ProjectReleases.FindAsync(id);
    if (release is null) return;
    release.ProjectId = vm.ProjectId;
    release.Version = vm.Version;
    release.ReleaseDate = vm.ReleaseDate;
    release.DownloadUrl = vm.DownloadUrl;
    release.Notes = vm.Notes;
    await db.SaveChangesAsync();
}

internal async Task DeleteReleaseAsync(Guid id)
{
    var release = await db.ProjectReleases.FindAsync(id);
    if (release is null) return;
    db.ProjectReleases.Remove(release);
    await db.SaveChangesAsync();
}

internal Task<List<BlogPost>> GetAllBlogPostsAsync()
{
    return db.BlogPosts.Include(b => b.Project).OrderByDescending(b => b.PublishedAt).ToListAsync();
}

internal Task<BlogPost?> GetBlogPostByIdAsync(Guid id)
{
    return db.BlogPosts.FirstOrDefaultAsync(b => b.Id == id);
}

internal async Task<BlogPost> CreateBlogPostAsync(BlogPostFormViewModel vm)
{
    var post = new BlogPost
    {
        Id = Guid.NewGuid(),
        ProjectId = vm.ProjectId,
        Title = vm.Title,
        Slug = vm.Slug,
        Body = vm.Body,
        PublishedAt = vm.PublishedAt
    };
    db.BlogPosts.Add(post);
    await db.SaveChangesAsync();
    return post;
}

internal async Task UpdateBlogPostAsync(Guid id, BlogPostFormViewModel vm)
{
    var post = await db.BlogPosts.FindAsync(id);
    if (post is null) return;
    post.ProjectId = vm.ProjectId;
    post.Title = vm.Title;
    post.Slug = vm.Slug;
    post.Body = vm.Body;
    post.PublishedAt = vm.PublishedAt;
    await db.SaveChangesAsync();
}

internal async Task DeleteBlogPostAsync(Guid id)
{
    var post = await db.BlogPosts.FindAsync(id);
    if (post is null) return;
    db.BlogPosts.Remove(post);
    await db.SaveChangesAsync();
}

internal Task<List<ChangelogEntry>> GetAllChangelogEntriesAsync()
{
    return db.ChangelogEntries.Include(c => c.Project).OrderByDescending(c => c.Date).ToListAsync();
}

internal Task<ChangelogEntry?> GetChangelogEntryByIdAsync(Guid id)
{
    return db.ChangelogEntries.FirstOrDefaultAsync(c => c.Id == id);
}

internal async Task<ChangelogEntry> CreateChangelogEntryAsync(ChangelogFormViewModel vm)
{
    var entry = new ChangelogEntry
    {
        Id = Guid.NewGuid(),
        ProjectId = vm.ProjectId,
        Date = vm.Date,
        Description = vm.Description
    };
    db.ChangelogEntries.Add(entry);
    await db.SaveChangesAsync();
    return entry;
}

internal async Task UpdateChangelogEntryAsync(Guid id, ChangelogFormViewModel vm)
{
    var entry = await db.ChangelogEntries.FindAsync(id);
    if (entry is null) return;
    entry.ProjectId = vm.ProjectId;
    entry.Date = vm.Date;
    entry.Description = vm.Description;
    await db.SaveChangesAsync();
}

internal async Task DeleteChangelogEntryAsync(Guid id)
{
    var entry = await db.ChangelogEntries.FindAsync(id);
    if (entry is null) return;
    db.ChangelogEntries.Remove(entry);
    await db.SaveChangesAsync();
}
}