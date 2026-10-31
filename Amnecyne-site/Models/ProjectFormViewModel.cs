using System.ComponentModel.DataAnnotations;
using Amnecyne.DataModels;

namespace Amnecyne.Models;

public class ProjectFormViewModel
{
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = "";

    [DataType(DataType.MultilineText)]
    public string Description { get; set; } = "";

    [Required]
    public string Slug { get; set; } = "";

    [UIHint("ColorPicker")]
    public string PrimaryColor { get; set; } = "#3b82f6";

    [Required]
    public ProjectState State { get; set; }

    public GitVisibility GithubVisibility { get; set; } = GitVisibility.Private;

    [Display(Name = "GitHub Link")]
    public string? GithubLink { get; set; }

    public bool IsFeatured { get; set; }

    [UIHint("TagPicker")]
    public List<Guid> SelectedTagIds { get; set; } = new();

    // display-only, not posted back from the main form
    public string? IconPath { get; set; }
    public List<ProjectImage> Images { get; set; } = new();
    public List<ProjectRelease> Releases { get; set; } = new();
    public List<BlogPost> BlogPosts { get; set; } = new();

    public static ProjectFormViewModel FromEntity(Project p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Description = p.Description,
        Slug = p.Slug,
        PrimaryColor = p.PrimaryColor,
        State = p.State,
        GithubVisibility = p.GithubVisibility,
        GithubLink = p.GithubLink,
        IsFeatured = p.IsFeatured,
        SelectedTagIds = p.Tags.Select(t => t.Id).ToList(),
        IconPath = p.IconPath,
        Images = p.Images,
        Releases = p.Releases,
        BlogPosts = p.BlogPosts
    };
}