using Amnecyne.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Amnecyne.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        var tagCache = new Dictionary<string, Tag>(StringComparer.OrdinalIgnoreCase);

        Tag Tag(string name) => tagCache.TryGetValue(name, out var t)
            ? t
            : tagCache[name] = new Tag { Id = Guid.NewGuid(), Name = name, Slug = Slugify(name) };

        var projects = new List<Project>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Slug = "cacheit",
                Name = "CacheIt",
                Description = "Note-taking app with real-time sync via WebSockets, built zero-trust with end-to-end encryption.",
                State = ProjectState.Shipped,
                Tags = [Tag("Kotlin"), Tag("Spring Boot"), Tag("Jetpack Compose"), Tag("Avalonia"), Tag("PostgreSQL")],
                GithubVisibility = GitVisibility.Public,
                UpdatedAt = DateTime.SpecifyKind(new DateTime(2026, 10, 11), DateTimeKind.Utc),
                IsFeatured = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                Slug = "keyring",
                Name = "Keyring",
                Description = "Credentials manager with auto-login functionality.",
                State = ProjectState.Concept,
                Tags = [Tag(".NET MAUI"), Tag("React"), Tag("shadcn/ui"), Tag("FastAPI"), Tag("PostgreSQL")],
                GithubVisibility = GitVisibility.Private,
                UpdatedAt = DateTime.SpecifyKind(new DateTime(2026, 5, 1), DateTimeKind.Utc),
                IsFeatured = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                Slug = "vaultport",
                Name = "VaultPort",
                Description = "NAS management container with filesync functionality.",
                State = ProjectState.Shelved,
                Tags = [Tag(".NET 10"), Tag("EF Core")],
                GithubVisibility = GitVisibility.Private,
                UpdatedAt = DateTime.SpecifyKind(new DateTime(2025, 11, 1), DateTimeKind.Utc),
                IsFeatured = false
            },
            new()
            {
                Id = Guid.NewGuid(),
                Slug = "musilink",
                Name = "Musilink",
                Description = "Music playlist sharing and shared listening app with collaborative, community-driven elements.",
                State = ProjectState.Concept,
                Tags = [Tag("Kotlin"), Tag("Spring Boot"), Tag("Jetpack Compose"), Tag("PostgreSQL")],
                GithubVisibility = GitVisibility.Untracked,
                IsFeatured = false
            }
        };

        foreach (var project in projects)
        {
            if (!await db.Projects.AnyAsync(p => p.Slug == project.Slug))
            {
                db.Projects.Add(project);
            }
        }

        await db.SaveChangesAsync();
    }

    private static string Slugify(string name) =>
        name.ToLowerInvariant()
            .Replace(".", "")
            .Replace("+", "-plus")
            .Replace("/", "-")
            .Replace(" ", "-");
}