namespace Amnecyne.DataModels;

public class ProjectRelease
{
    public required Guid Id { get; set; }
    public required Guid ProjectId { get; set; }
    public Project? Project { get; set; }
    public required string Version { get; set; }
    public required DateTime ReleaseDate { get; set; }
    public required string DownloadUrl { get; set; }
    public string? Notes { get; set; }
}