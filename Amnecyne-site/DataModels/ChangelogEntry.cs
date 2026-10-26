namespace Amnecyne.DataModels;

public class ChangelogEntry
{
    public Guid Id { get; set; } //primary for  the changelog itself
    public required Guid ProjectId { get; set; } // FK: for project link
    public required DateTime Date { get; set; }
    public required string Description { get; set; }
}