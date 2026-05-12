using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HildaDefaultSetsSchemaTests;

public class DefaultManifest
{
    public string Version { get; set; } = "";
    public string LastUpdated { get; set; } = "";
    public Dictionary<int, List<SetManifest>> Sets { get; set; } = new();
}

public class SetManifest
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";
    public string Md5 { get; set; } = "";
    public string LastUpdated { get; set; } = "";
    public string Version { get; set; } = "";
    public string AppVersion { get; set; } = "";
    public bool? SubSet { get; set; }
}

internal static class ManifestLoader
{
    public static DefaultManifest Load()
    {
        var json = File.ReadAllText(TestPaths.ManifestPath);
        var manifest = JsonConvert.DeserializeObject<DefaultManifest>(json)
            ?? throw new InvalidOperationException("manifest.json deserialized to null");
        return manifest;
    }

    public static JObject LoadSetJson(string filePath)
    {
        var json = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<JObject>(json)
            ?? throw new InvalidOperationException($"{filePath} deserialized to null");
    }
}
