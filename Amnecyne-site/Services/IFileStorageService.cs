namespace Amnecyne.Services;

public interface IFileStorageService
{
    /// Saves the file under wwwroot/uploads/projects/{projectId}/{subfolder}/
    /// and returns the relative path to store in the DB (e.g. "projects/{id}/icon.png").
    Task<string> SaveAsync(Guid projectId, string subfolder, string fileNameWithoutExt, IFormFile file, CancellationToken ct = default);

    void Delete(string relativePath);
}