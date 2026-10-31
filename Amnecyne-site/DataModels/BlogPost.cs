namespace Amnecyne.DataModels;

public class BlogPost
{
    public required Guid Id { get; set; }
    public required Guid ProjectId { get; set; }
    public Project? Project { get; set; }
    public required string Title { get; set; }
    public required string Slug { get; set; }
    public required string Body { get; set; }
    public required DateTime PublishedAt { get; set; }
}