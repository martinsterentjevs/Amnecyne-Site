namespace Amnecyne.DataModels;

public class ProjectImage
{
    public required Guid Id { get; set; }
    public required Guid ProjectId { get; set; }
    public Project? Project { get; set; }
    public required string Path { get; set; }   // relative path under wwwroot/uploads, not a full URL
    public string? Caption { get; set; }
    public int SortOrder { get; set; }
}