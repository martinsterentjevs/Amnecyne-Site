using Amnecyne.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Amnecyne.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options):DbContext (options)
{
    public DbSet<Project>  Projects { get; set; }
    public DbSet<ChangelogEntry>  ChangelogEntries { get; set; }
    public DbSet<Tag> Tags { get; set; }
}