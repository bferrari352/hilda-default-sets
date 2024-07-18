using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HildaDefaultSetsApp;

internal static class ManifestGenerator
{
    public static void Main(string[] args)
    {
        var currentManifest = GetCurrentManifest();
        if (currentManifest == null)
        {
            Console.WriteLine("Error getting current manifest");
            throw new Exception();
        }
        
        var newManifest = new DefaultManifest
        {
            Version = currentManifest.Version,
            LastUpdated = currentManifest.LastUpdated,
            Sets = new()
        };

        // Go through each folder in //priorities
        var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (assemblyPath == null)
        {
            Console.WriteLine("No Assembly Path!");
            return;
        }
        
        var prioritiesDir = Path.Combine(assemblyPath, "priorities");
        var jobDirectories = Directory.GetDirectories(prioritiesDir).OrderBy(x => x);
        
        foreach (var jobDirectoryPath in jobDirectories)
        {
            var directories = jobDirectoryPath.Split(Path.DirectorySeparatorChar).ToList();
            var setFiles = Directory.GetFiles(jobDirectoryPath);
            if (!int.TryParse(directories[^1], out var jobId)) continue;
            
            foreach (var setFile in setFiles)
            {
                var fileString = File.ReadAllText(setFile);
                var newSet = GetSetData(fileString);
                if (newManifest.Sets.TryGetValue(jobId, out var value))
                {
                    value.Add(newSet);
                }
                else
                {
                    newManifest.Sets[jobId] = [newSet];
                }
            }
        }
        
        if (args.Contains("+version"))
        {
            var strings = newManifest.Version.Split(".");
            var versioning = Array.ConvertAll(strings, int.Parse);
            if (args.Contains("major"))
            {
                versioning[0] += 1;
                versioning[1] = 0;
                versioning[2] = 0;
            }
            else if (args.Contains("minor"))
            {
                versioning[1] += 1;
                versioning[2] = 0;
            }
            else
            {
                versioning[2] += 1;
            }
        
            var versionString = Array.ConvertAll(versioning, s => s.ToString());
            newManifest.Version = string.Join(".", versionString);
        }
        
        newManifest.LastUpdated = DateTime.Now.ToString("u");
        
        WriteManifest(newManifest);
        Console.Write("Generated new manifest.json");
    }
        
    private static string GenerateMd5(string input)
    {
        using var md5 = MD5.Create();
        var inputBytes = Encoding.ASCII.GetBytes(input);
        var hashBytes = md5.ComputeHash(inputBytes);

        var sb = new StringBuilder();
        foreach (var t in hashBytes)
        {
            sb.Append(t.ToString("X2"));
        }
        return sb.ToString();
    }

    private static SetManifest GetSetData(string fileString)
    {
        var deserialized = JsonConvert.DeserializeObject<JObject>(fileString);
        if (deserialized == null)
        {
            throw new Exception($"Error acquiring set data in {fileString}");
        }

        var id = deserialized["id"] ?? deserialized["Id"];
        var name = deserialized["name"] ?? deserialized["Name"];
        var version = deserialized["version"] ?? deserialized["Version"];
        var appVersion = deserialized["appVersion"] ?? deserialized["AppVersion"];
        var lastUpdated = deserialized["lastUpdated"] ?? deserialized["LastUpdated"];
        var subSet = deserialized["subSet"] ?? deserialized["SubSet"];
        var priorities = deserialized["priorities"] ?? deserialized["Priorities"];
        var actions = deserialized["actions"] ?? deserialized["Actions"];

        if (id == null || name == null || version == null || appVersion == null || lastUpdated == null)
        {
            throw new Exception($"Data is null in {deserialized}");
        }

        var type = SetType.Unknown;
        if (priorities != null) {
            type = SetType.Priorities;
        } else if (actions != null) {
            type = SetType.Actions;
        }

        var manifest = new SetManifest
        {
            Id = id.ToString(),
            Name = name.ToString(),
            Type = type.ToString(),
            AppVersion = appVersion.ToString(),
            Version = version.ToString(),
            LastUpdated = lastUpdated.ToString(),
            Md5 = GenerateMd5(fileString)
        };
        
        if (subSet != null && subSet.Value<bool>())
        {
            manifest.SubSet = true;
        }
        
        return manifest;
    }
        
    private static DefaultManifest? GetCurrentManifest()
    {
        try
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (assemblyPath == null) return null;
            var filePath = Path.Combine(assemblyPath, "priorities/manifest.json");

            if (File.Exists(filePath))
            {
                var jsonString = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<DefaultManifest>(jsonString);
            }
        }
        catch (Exception e)
        {
            Console.Write(e.Message);
            throw;
        }
        
        return null;
    }
    
    private static void WriteManifest(DefaultManifest manifest)
    {
        try
        {
            var dir = Path.Combine(Directory.GetCurrentDirectory(), @"priorities");
            var filePath = Path.Combine(dir, @"manifest.json");

            if (!Directory.Exists(dir)) return;
            
            var serializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented};
            var jsonString = JsonConvert.SerializeObject(manifest, serializerSettings);
            File.WriteAllText(filePath, jsonString);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
        
}

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
    public string Type { get; set; } = SetType.Unknown.ToString();
    public string Md5 { get; set; } = "";
    public string LastUpdated { get; set; } = "";
    public string Version { get; set; } = "";
    public string AppVersion { get; set; } = "";
    public bool? SubSet { get; set; }
}

public enum SetType {
    Priorities,
    Actions,
    Unknown
}