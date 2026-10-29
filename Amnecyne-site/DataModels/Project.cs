namespace Amnecyne.DataModels;

public class Project
{
    public required Guid Id { get; set; }
    public string PrimaryColor { get; set; } = "#3b82f6";
    public required string Name { get; set; }
    public string Description { get; set; } = "";
    public required ProjectState State { get; set; }
    public GitVisibility GithubVisibility { get; set; } = GitVisibility.Private;
    public string? GithubLink { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public required List<Tag> Tags { get; set; } = new();
    public string? LatestRelease { get; set; } 
    public required string Slug { get; set; }
    public required bool IsFeatured { get; set; } = false;

    public string? IconPath { get; set; }
    public List<ProjectImage> Images { get; set; } = new();
}

public enum GitVisibility
{
    Public,
    Private,
    Untracked
}

public enum ProjectState
{
    Active,
    Shelved,
    Concept,
    Shipped
}