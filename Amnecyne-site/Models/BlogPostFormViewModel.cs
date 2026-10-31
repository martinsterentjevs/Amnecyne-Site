using System.ComponentModel.DataAnnotations;

namespace Amnecyne.Models;

public class BlogPostFormViewModel
{
    public Guid Id { get; set; }

    [UIHint("EntityReference")]
    [Display(Name = "Project")]
    public Guid ProjectId { get; set; }

    [Required]
    public string Title { get; set; } = "";

    [Required]
    public string Slug { get; set; } = "";

    [Required]
    [DataType(DataType.MultilineText)]
    public string Body { get; set; } = "";

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Published")]
    public DateTime PublishedAt { get; set; } = DateTime.UtcNow;

    public static BlogPostFormViewModel FromEntity(Amnecyne.DataModels.BlogPost b) => new()
    {
        Id = b.Id,
        ProjectId = b.ProjectId,
        Title = b.Title,
        Slug = b.Slug,
        Body = b.Body,
        PublishedAt = b.PublishedAt
    };
}