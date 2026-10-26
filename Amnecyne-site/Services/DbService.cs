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