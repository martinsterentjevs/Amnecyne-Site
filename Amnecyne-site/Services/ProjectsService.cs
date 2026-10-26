using Amnecyne.DataModels;
using Amnecyne.Models;

namespace Amnecyne.Services;

public class ProjectsService(DbService dbService, ILogger<ProjectsService> logger)
{
    public Task<Project?> GetBySlugId(string slugId) => dbService.GetBySlugId(slugId);

    public async Task<List<Project>> GetAllAsync()
    {
        return await dbService.GetAllAsync();
    }
    /*public async Task<List<ProjectCardViewModel>> GetByTagAsync(string tag)
    {
        return await dbService.GetByTagAsync(tag);
    }*/
    public async Task<List<ChangelogEntry>> GetChangelogAsync(Guid projectId)
        => await dbService.GetChangelogAsync(projectId);
    
    public async Task<List<ProjectCardViewModel>> GetFeaturedAsync() => await dbService.GetFeaturedAsync();
}