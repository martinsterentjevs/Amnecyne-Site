using Microsoft.AspNetCore.Hosting;

namespace Amnecyne.Services;

public class LocalFileStorageService(IWebHostEnvironment env) : IFileStorageService
{
    private readonly string _root = Path.Combine(env.WebRootPath, "uploads");
    private const long MaxBytes = 5 * 1024 * 1024; // 5 MB

    private static readonly Dictionary<string, byte[]> MagicBytes = new()
    {
        [".png"]  = new byte[] { 0x89, 0x50, 0x4E, 0x47 },
        [".jpg"]  = new byte[] { 0xFF, 0xD8, 0xFF },
        [".jpeg"] = new byte[] { 0xFF, 0xD8, 0xFF },
        // webp checked separately below (RIFF....WEBP)
    };

    public async Task<string> SaveAsync(Guid projectId, string subfolder, string fileNameWithoutExt, IFormFile file, CancellationToken ct = default)
    {
        if (file.Length == 0 || file.Length > MaxBytes)
            throw new InvalidOperationException("File is empty or exceeds the 5 MB limit.");

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (ext is not (".png" or ".jpg" or ".jpeg" or ".webp"))
            throw new InvalidOperationException("Only PNG, JPEG, or WEBP images are allowed.");

        await using (var stream = file.OpenReadStream())
        {
            if (!await LooksLikeDeclaredTypeAsync(stream, ext, ct))
                throw new InvalidOperationException("File content doesn't match its extension.");
        }

        var relativeDir = Path.Combine("projects", projectId.ToString(), subfolder);
        var absoluteDir = Path.Combine(_root, relativeDir);
        Directory.CreateDirectory(absoluteDir);

        var fileName = $"{fileNameWithoutExt}{ext}";
        var absolutePath = Path.Combine(absoluteDir, fileName);

        await using (var dest = new FileStream(absolutePath, FileMode.Create))
        {
            await using var src = file.OpenReadStream();
            await src.CopyToAsync(dest, ct);
        }

        return Path.Combine(relativeDir, fileName).Replace('\\', '/');
    }
    

    public void Delete(string relativePath)
    {
        var absolutePath = Path.Combine(_root, relativePath.Replace('/', Path.DirectorySeparatorChar));
        if (File.Exists(absolutePath))
            File.Delete(absolutePath);
    }

    private static async Task<bool> LooksLikeDeclaredTypeAsync(Stream stream, string ext, CancellationToken ct)
    {
        var header = new byte[12];
        var read = await stream.ReadAsync(header.AsMemory(0, 12), ct);
        stream.Position = 0;
        if (read < 4) return false;

        if (ext == ".webp")
            return read >= 12
                && header[0] == 'R' && header[1] == 'I' && header[2] == 'F' && header[3] == 'F'
                && header[8] == 'W' && header[9] == 'E' && header[10] == 'B' && header[11] == 'P';

        return MagicBytes.TryGetValue(ext, out var expected)
            && header.Take(expected.Length).SequenceEqual(expected);
    }
}