using System.ComponentModel.DataAnnotations;
using Amnecyne.DataModels;

namespace Amnecyne.Models;

public class TagFormViewModel
{
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = "";

    [Required]
    public string Slug { get; set; } = "";

    public static TagFormViewModel FromEntity(Tag t) => new()
    {
        Id = t.Id,
        Name = t.Name,
        Slug = t.Slug
    };
}