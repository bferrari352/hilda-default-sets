using Xunit;

namespace HildaDefaultSetsSchemaTests;

public class ManifestConsistencyTests
{
    public static IEnumerable<object[]> AllManifestEntries()
    {
        var manifest = ManifestLoader.Load();
        foreach (var (jobId, sets) in manifest.Sets)
        {
            foreach (var set in sets)
            {
                yield return new object[] { jobId, set };
            }
        }
    }

    public static IEnumerable<object[]> AllPriorityFiles()
    {
        foreach (var jobDir in Directory.GetDirectories(TestPaths.PrioritiesDir))
        {
            var dirName = Path.GetFileName(jobDir);
            if (!int.TryParse(dirName, out var jobId)) continue;
            foreach (var file in Directory.GetFiles(jobDir, "*.json"))
            {
                yield return new object[] { jobId, file };
            }
        }
    }

    [Fact]
    public void Every_priority_json_file_has_a_manifest_entry()
    {
        var manifest = ManifestLoader.Load();

        var manifestIds = manifest.Sets
            .SelectMany(kvp => kvp.Value.Select(s => (kvp.Key, s.Id)))
            .ToHashSet();

        var orphans = new List<string>();
        foreach (var jobDir in Directory.GetDirectories(TestPaths.PrioritiesDir))
        {
            if (!int.TryParse(Path.GetFileName(jobDir), out var jobId)) continue;
            foreach (var file in Directory.GetFiles(jobDir, "*.json"))
            {
                var json = ManifestLoader.LoadSetJson(file);
                var id = (json["id"] ?? json["Id"])?.ToString() ?? "";
                if (!manifestIds.Contains((jobId, id)))
                {
                    orphans.Add($"{jobId}/{Path.GetFileName(file)} (id={id})");
                }
            }
        }

        Assert.True(orphans.Count == 0,
            "Priority JSON files exist on disk but are missing from manifest.json:\n  " +
            string.Join("\n  ", orphans));
    }

    [Theory]
    [MemberData(nameof(AllManifestEntries))]
    public void Manifest_entry_has_matching_file_on_disk(int jobId, SetManifest set)
    {
        var jobDir = Path.Combine(TestPaths.PrioritiesDir, jobId.ToString());
        Assert.True(Directory.Exists(jobDir),
            $"Manifest references jobId {jobId} but priorities/{jobId}/ does not exist");

        var match = Directory.GetFiles(jobDir, "*.json")
            .Select(f => (Path: f, Json: ManifestLoader.LoadSetJson(f)))
            .Where(x => ((x.Json["id"] ?? x.Json["Id"])?.ToString() ?? "") == set.Id)
            .ToList();

        Assert.True(match.Count == 1,
            $"Manifest entry {jobId}/{set.Id} ({set.Name}) has {match.Count} matching files on disk; expected 1");
    }

    [Theory]
    [MemberData(nameof(AllManifestEntries))]
    public void Manifest_md5_matches_file_on_disk(int jobId, SetManifest set)
    {
        var file = FindFileById(jobId, set.Id);
        var actual = TestPaths.ComputeMd5(file);

        Assert.True(string.Equals(actual, set.Md5, StringComparison.OrdinalIgnoreCase),
            $"MD5 mismatch for {jobId}/{Path.GetFileName(file)} ({set.Name}): " +
            $"manifest says {set.Md5}, actual is {actual}. " +
            $"Run `dotnet run --project HildaDefaultSets` to regenerate the manifest.");
    }

    [Theory]
    [MemberData(nameof(AllManifestEntries))]
    public void Manifest_metadata_matches_set_json(int jobId, SetManifest set)
    {
        var file = FindFileById(jobId, set.Id);
        var json = ManifestLoader.LoadSetJson(file);

        var fileName = (json["name"] ?? json["Name"])?.ToString() ?? "";
        var fileVersion = (json["version"] ?? json["Version"])?.ToString() ?? "";
        var fileAppVersion = (json["appVersion"] ?? json["AppVersion"])?.ToString() ?? "";
        var fileLastUpdated = (json["lastUpdated"] ?? json["LastUpdated"])?.ToString() ?? "";

        Assert.Equal(set.Name, fileName);
        Assert.Equal(set.Version, fileVersion);
        Assert.Equal(set.AppVersion, fileAppVersion);
        Assert.Equal(set.LastUpdated, fileLastUpdated);
    }

    [Theory]
    [MemberData(nameof(AllPriorityFiles))]
    public void Set_json_jobId_matches_directory(int dirJobId, string file)
    {
        var json = ManifestLoader.LoadSetJson(file);
        var fileJobId = (int?)(json["jobId"] ?? json["JobId"]);

        Assert.True(fileJobId == dirJobId,
            $"{file}: directory says jobId={dirJobId} but file has jobId={fileJobId}");
    }

    private static string FindFileById(int jobId, string id)
    {
        var jobDir = Path.Combine(TestPaths.PrioritiesDir, jobId.ToString());
        return Directory.GetFiles(jobDir, "*.json")
            .First(f =>
            {
                var json = ManifestLoader.LoadSetJson(f);
                return ((json["id"] ?? json["Id"])?.ToString() ?? "") == id;
            });
    }
}
