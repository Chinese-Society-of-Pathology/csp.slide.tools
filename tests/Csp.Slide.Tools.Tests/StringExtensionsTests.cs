using System.IO;
using Csp.Slide.Tools;

namespace Csp.Slide.Tools.Tests;

public class StringExtensionsTests
{
    // ── ArePathsSame ─────────────────────────────────────────────────────────

    [Fact]
    public void ArePathsSame_IdenticalPaths_ReturnsTrue()
    {
        var path = Path.GetTempPath();
        Assert.True(path.ArePathsSame(path));
    }

    [Fact]
    public void ArePathsSame_DifferentCasing_ReturnsTrue()
    {
        var lower = Path.GetTempPath().ToLowerInvariant();
        var upper = Path.GetTempPath().ToUpperInvariant();
        Assert.True(lower.ArePathsSame(upper));
    }

    [Fact]
    public void ArePathsSame_DifferentPaths_ReturnsFalse()
    {
        var path1 = Path.GetTempPath();
        var path2 = Path.GetFullPath(".");
        Assert.False(path1.ArePathsSame(path2));
    }

    // ── IsFolder ─────────────────────────────────────────────────────────────

    [Fact]
    public void IsFolder_ExistingDirectory_ReturnsTrue()
    {
        var dir = Path.GetTempPath();
        Assert.True(dir.IsFolder());
    }

    [Fact]
    public void IsFolder_ExistingFile_ReturnsFalse()
    {
        var file = Path.GetTempFileName();
        try
        {
            Assert.False(file.IsFolder());
        }
        finally
        {
            File.Delete(file);
        }
    }

    [Fact]
    public void IsFolder_NonExistentPath_ReturnsFalse()
    {
        var nonExistent = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Assert.False(nonExistent.IsFolder());
    }

    // ── GetFiles ─────────────────────────────────────────────────────────────

    [Fact]
    public void GetFiles_DirectoryWithFiles_ReturnsAllFiles()
    {
        var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(dir);
        var subDir = Path.Combine(dir, "sub");
        Directory.CreateDirectory(subDir);

        var file1 = Path.Combine(dir, "a.txt");
        var file2 = Path.Combine(subDir, "b.txt");
        File.WriteAllText(file1, "x");
        File.WriteAllText(file2, "x");

        try
        {
            var files = dir.GetFiles().Select(f => f.FullName).ToList();
            Assert.Contains(file1, files);
            Assert.Contains(file2, files);
            Assert.Equal(2, files.Count);
        }
        finally
        {
            Directory.Delete(dir, recursive: true);
        }
    }

    [Fact]
    public void GetFiles_EmptyDirectory_ReturnsEmpty()
    {
        var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(dir);
        try
        {
            Assert.Empty(dir.GetFiles());
        }
        finally
        {
            Directory.Delete(dir);
        }
    }

    // ── RenameSameNameFiles ───────────────────────────────────────────────────

    [Fact]
    public void RenameSameNameFiles_NoDuplicates_ReturnsSameNames()
    {
        var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(dir);

        var file1 = Path.Combine(dir, "alpha.sdpc");
        var file2 = Path.Combine(dir, "beta.sdpc");
        File.WriteAllText(file1, "");
        File.WriteAllText(file2, "");

        try
        {
            var result = dir.RenameSameNameFiles();
            Assert.All(result, t => Assert.Equal(t.Item1, t.Item2));
        }
        finally
        {
            Directory.Delete(dir, recursive: true);
        }
    }

    [Fact]
    public void RenameSameNameFiles_DuplicateBasenames_AssignsUniqueNames()
    {
        var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        var subDir = Path.Combine(dir, "sub");
        Directory.CreateDirectory(subDir);

        var file1 = Path.Combine(dir, "slide.sdpc");
        var file2 = Path.Combine(subDir, "slide.dcm");
        File.WriteAllText(file1, "");
        File.WriteAllText(file2, "");

        try
        {
            var result = dir.RenameSameNameFiles();
            var destNames = result.Select(t => Path.GetFileNameWithoutExtension(t.Item2)).ToList();

            // Both entries originate from "slide", at least one must be renamed (suffixed).
            Assert.Contains(destNames, n => n != "slide");
        }
        finally
        {
            Directory.Delete(dir, recursive: true);
        }
    }
}
