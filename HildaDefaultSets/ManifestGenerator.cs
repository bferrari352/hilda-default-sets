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

        var sets = new Dictionary<int, List<SetManifest>>(currentManifest.Sets);
        foreach (var (jobId, setManifests) in sets)
        {
            for (var index = 0; index < setManifests.Count; index++)
            {
                var setManifest = setManifests[index];
                var fileString = GetPrioritySet(jobId, setManifest.Id);
                if (fileString == null)
                {
                    Console.WriteLine($"SetID: '{setManifest.Id}' could not be found for JobID: {jobId}");
                    throw new Exception();
                }

                var md5 = GenerateMd5(fileString);
                var (version, appVersion, lastUpdated) = GetSetData(fileString);

                if (md5 != setManifest.Md5)
                {
                    Console.WriteLine($"Updating MD5 for JobID: {jobId} -> SetID: {setManifest.Id}");
                    currentManifest.Sets[jobId][index] = new SetManifest
                    {
                        Id = setManifest.Id,
                        LastUpdated = lastUpdated,
                        Md5 = md5,
                        Version = version,
                        AppVersion = appVersion,
                    };
                }
            }
        }
        
        if (args.Contains("+version"))
        {
            var strings = currentManifest.Version.Split(".");
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
            currentManifest.Version = string.Join(".", versionString);
        }

        currentManifest.LastUpdated = DateTime.Now.ToString("u");
        WriteManifest(currentManifest);
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

    private static (string, string, string) GetSetData(string fileString)
    {
        var deserialized = JsonConvert.DeserializeObject<JObject>(fileString);
        if (deserialized == null)
        {
            throw new Exception("Error acquiring set data");
        }
        return (deserialized["version"].ToString(), deserialized["appVersion"].ToString(), deserialized["lastUpdated"].ToString());
    }

    private static string? GetPrioritySet(int jobId, string setId)
    {
        try
        {
            var assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var dir = Path.Combine(Path.GetDirectoryName(assemblyLocation)!, "priorities", jobId.ToString());
            var filePath = Path.Combine(dir, @$"{setId}.json");

            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
        }
        catch
        {
            Console.WriteLine("Error getting priority set");
            throw;
        }

        return null;
    }
        
    private static DefaultManifest? GetCurrentManifest()
    {
        try
        {
            var assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var filePath = Path.Combine(Path.GetDirectoryName(assemblyLocation)!,
                @"priorities/manifest.json");

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
    
    private static bool WriteManifest(DefaultManifest manifest)
    {
        try
        {
            var dir = Path.Combine(Directory.GetCurrentDirectory(), @"priorities");
            var filePath = Path.Combine(dir, @"manifest.json");

            if (Directory.Exists(dir))
            {
                var serializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore,
                    Formatting = Formatting.Indented};
                var jsonString = JsonConvert.SerializeObject(manifest, serializerSettings);
                File.WriteAllText(filePath, jsonString);
                return true;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }

        return false;
    }
        
}

public class DefaultManifest
{
    public string Version { get; set; }
    public string LastUpdated { get; set; }
    public Dictionary<int, List<SetManifest>> Sets { get; set; }

    public DefaultManifest()
    {
        Version = "";
        LastUpdated = "";
        Sets = new Dictionary<int, List<SetManifest>>();
    }
}

public class SetManifest
{
    public string Id { get; set; }
    public string Md5 { get; set; }
    public string LastUpdated { get; set; }
    public string Version { get; set; }
    public string AppVersion { get; set; }

    public SetManifest()
    {
        Id = "";
        Md5 = "";
        LastUpdated = "";
        Version = "";
        AppVersion = "";
    }
}