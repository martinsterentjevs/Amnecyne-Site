using System.ComponentModel.DataAnnotations;
using Amnecyne.DataModels;

namespace Amnecyne.Models;

public class ChangelogFormViewModel
{
    public Guid Id { get; set; }

    [UIHint("EntityReference")]
    [Display(Name = "Project")]
    public Guid ProjectId { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; } = DateTime.UtcNow;

    [Required]
    [DataType(DataType.MultilineText)]
    public string Description { get; set; } = "";

    public static ChangelogFormViewModel FromEntity(ChangelogEntry c) => new()
    {
        Id = c.Id,
        ProjectId = c.ProjectId,
        Date = c.Date,
        Description = c.Description
    };
}