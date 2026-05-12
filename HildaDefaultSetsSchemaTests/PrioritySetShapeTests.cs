using System.Text.RegularExpressions;
using Xunit;

namespace HildaDefaultSetsSchemaTests;

public class PrioritySetShapeTests
{
    private static readonly Regex SemVerThree = new(@"^\d+\.\d+\.\d+$", RegexOptions.Compiled);
    private static readonly Regex DottedVersion = new(@"^\d+\.\d+(\.\d+){0,2}$", RegexOptions.Compiled);

    public static IEnumerable<object[]> AllPriorityFiles() => ManifestConsistencyTests.AllPriorityFiles();

    [Theory]
    [MemberData(nameof(AllPriorityFiles))]
    public void Set_json_has_required_fields(int jobId, string file)
    {
        _ = jobId;
        var json = ManifestLoader.LoadSetJson(file);

        foreach (var field in new[] { "id", "name", "jobId", "version", "appVersion", "lastUpdated" })
        {
            var present = json[field] != null || json[char.ToUpper(field[0]) + field[1..]] != null;
            Assert.True(present, $"{file}: missing required field '{field}'");
        }

        var hasPriorities = json["priorities"] != null || json["Priorities"] != null;
        var hasActions = json["actions"] != null || json["Actions"] != null;
        Assert.True(hasPriorities || hasActions,
            $"{file}: must contain either 'priorities' or 'actions'");
    }

    [Theory]
    [MemberData(nameof(AllPriorityFiles))]
    public void Set_json_version_is_semver(int jobId, string file)
    {
        _ = jobId;
        var json = ManifestLoader.LoadSetJson(file);
        var version = (json["version"] ?? json["Version"])?.ToString() ?? "";

        Assert.True(SemVerThree.IsMatch(version),
            $"{file}: version '{version}' is not in N.N.N form");
    }

    [Theory]
    [MemberData(nameof(AllPriorityFiles))]
    public void Set_json_appVersion_is_dotted_version(int jobId, string file)
    {
        _ = jobId;
        var json = ManifestLoader.LoadSetJson(file);
        var appVersion = (json["appVersion"] ?? json["AppVersion"])?.ToString() ?? "";

        Assert.True(DottedVersion.IsMatch(appVersion),
            $"{file}: appVersion '{appVersion}' is not a dotted version (N.N, N.N.N, or N.N.N.N)");
    }

    [Fact]
    public void Manifest_version_is_semver()
    {
        var manifest = ManifestLoader.Load();
        Assert.True(SemVerThree.IsMatch(manifest.Version),
            $"manifest.Version '{manifest.Version}' is not in N.N.N form");
    }

    [Fact]
    public void All_set_ids_are_unique_across_manifest()
    {
        var manifest = ManifestLoader.Load();
        var duplicates = manifest.Sets
            .SelectMany(kvp => kvp.Value.Select(s => s.Id))
            .GroupBy(id => id)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        Assert.True(duplicates.Count == 0,
            "Duplicate set IDs across manifest:\n  " + string.Join("\n  ", duplicates));
    }
}
