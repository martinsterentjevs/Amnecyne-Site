namespace Amnecyne.DataModels;

public class Tag
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }   // "PostgreSQL"
    public required string Slug { get; set; }   // "postgresql" — for clean URLs/filters

    public List<Project> Projects { get; set; } = new();
}