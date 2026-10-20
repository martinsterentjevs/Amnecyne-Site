using Amnecyne.DataModels;

namespace Amnecyne.Services;

public class ProjectsService(DbService dbService, ILogger<ProjectsService> logger)
{
    public Project? GetBySlugId(string slugId)
    {
        return (Project?)dbService.GetBySlugId(slugId);
    }

    public List<Project> GetAll()
    {
        return dbService.GetAll();
    }
    
}