using Amnecyne.Data;
using Amnecyne.DataModels;

namespace Amnecyne.Services;

public class DbService(AppDbContext db)
{
    internal List<Project> GetAll()
    {
        return [.. db.Projects];
    }
    
    internal object? GetBySlugId(string slug)
    {
        var project = db.Projects.FirstOrDefault(p => p.Slug == slug) ?? null;
        return project;
    }
}