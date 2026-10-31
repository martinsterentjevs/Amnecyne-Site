using System.ComponentModel.DataAnnotations;

namespace Amnecyne.Models;

public class ReleaseFormViewModel
{
    public Guid Id { get; set; }

    [UIHint("EntityReference")]
    [Display(Name = "Project")]
    public Guid ProjectId { get; set; }

    [Required]
    public string Version { get; set; } = "";

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Release date")]
    public DateTime ReleaseDate { get; set; } = DateTime.UtcNow;

    [Required]
    [Display(Name = "Download URL")]
    public string DownloadUrl { get; set; } = "";

    public string? Notes { get; set; }

    public static ReleaseFormViewModel FromEntity(Amnecyne.DataModels.ProjectRelease r) => new()
    {
        Id = r.Id,
        ProjectId = r.ProjectId,
        Version = r.Version,
        ReleaseDate = r.ReleaseDate,
        DownloadUrl = r.DownloadUrl,
        Notes = r.Notes
    };
}