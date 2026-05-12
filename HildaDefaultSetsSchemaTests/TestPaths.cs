using System.Security.Cryptography;
using System.Text;

namespace HildaDefaultSetsSchemaTests;

internal static class TestPaths
{
    private static readonly Lazy<string> _prioritiesDir = new(LocatePrioritiesDir);

    public static string PrioritiesDir => _prioritiesDir.Value;
    public static string ManifestPath => Path.Combine(PrioritiesDir, "manifest.json");

    private static string LocatePrioritiesDir()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null)
        {
            var candidate = Path.Combine(dir.FullName, "HildaDefaultSets", "priorities", "manifest.json");
            if (File.Exists(candidate))
            {
                return Path.GetDirectoryName(candidate)!;
            }
            dir = dir.Parent;
        }
        throw new InvalidOperationException(
            "Could not locate HildaDefaultSets/priorities/manifest.json by walking up from " +
            AppContext.BaseDirectory);
    }

    public static string ComputeMd5(string filePath)
    {
        var text = File.ReadAllText(filePath);
        var asciiBytes = Encoding.ASCII.GetBytes(text);
        var hash = MD5.HashData(asciiBytes);

        var sb = new StringBuilder(hash.Length * 2);
        foreach (var b in hash) sb.Append(b.ToString("X2"));
        return sb.ToString();
    }
}
