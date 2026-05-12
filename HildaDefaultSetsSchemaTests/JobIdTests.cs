using Xunit;

namespace HildaDefaultSetsSchemaTests;

public class JobIdTests
{
    [Fact]
    public void Priority_directories_are_all_numeric_job_ids()
    {
        var nonNumeric = Directory.GetDirectories(TestPaths.PrioritiesDir)
            .Select(Path.GetFileName)
            .Where(name => !int.TryParse(name, out _))
            .ToList();

        Assert.True(nonNumeric.Count == 0,
            "Non-numeric directories under priorities/: " + string.Join(", ", nonNumeric));
    }

    [Fact]
    public void Every_priority_directory_appears_in_manifest()
    {
        var manifest = ManifestLoader.Load();
        var manifestJobIds = manifest.Sets.Keys.ToHashSet();

        var dirJobIds = Directory.GetDirectories(TestPaths.PrioritiesDir)
            .Select(Path.GetFileName)
            .Where(name => int.TryParse(name, out _))
            .Select(name => int.Parse(name!))
            .ToHashSet();

        var missingFromManifest = dirJobIds.Except(manifestJobIds).ToList();
        Assert.True(missingFromManifest.Count == 0,
            "Job directories not in manifest: " + string.Join(", ", missingFromManifest));

        var missingFromDisk = manifestJobIds.Except(dirJobIds).ToList();
        Assert.True(missingFromDisk.Count == 0,
            "Manifest jobIds without a directory: " + string.Join(", ", missingFromDisk));
    }

    [Fact]
    public void Every_priority_directory_has_at_least_one_set()
    {
        var emptyDirs = Directory.GetDirectories(TestPaths.PrioritiesDir)
            .Where(d => int.TryParse(Path.GetFileName(d), out _))
            .Where(d => Directory.GetFiles(d, "*.json").Length == 0)
            .Select(Path.GetFileName)
            .ToList();

        Assert.True(emptyDirs.Count == 0,
            "Empty job directories: " + string.Join(", ", emptyDirs));
    }
}
