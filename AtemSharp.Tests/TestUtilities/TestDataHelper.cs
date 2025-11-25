using System.Reflection;
using AtemSharp.State.Info;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AtemSharp.Tests.TestUtilities;

/// <summary>
/// Helper class for loading and working with test data from embedded resources
/// </summary>
public static class TestDataHelper
{
    public class CommandTestData
    {
        public string Name { get; set; } = "";
        public ProtocolVersion FirstVersion { get; set; }
        public string Bytes { get; set; } = "";
        public JObject Command { get; set; } = new();
    }

    /// <summary>
    /// Load all test data from the embedded libatem-data.json resource file.
    /// </summary>
    public static CommandTestData[] LoadAllTestData()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "AtemSharp.Tests.TestData.libatem-data.json";

        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            throw new FileNotFoundException($"Could not find embedded resource: {resourceName}");
        }

        using var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();
        return JsonConvert.DeserializeObject<CommandTestData[]>(json) ?? Array.Empty<CommandTestData>();
    }

    /// <summary>
    /// Load test data for a specific command raw name
    /// </summary>
    public static CommandTestData[] LoadTestDataForCommand(string commandRawName)
    {
        var allTestData = LoadAllTestData();
        return allTestData.Where(tc => tc.Name == commandRawName).ToArray();
    }

    /// <summary>
    /// Parse a hex string (e.g., "01-02-03") into a byte array
    /// </summary>
    public static byte[] ParseHexBytes(string hexString)
    {
        return hexString.Split('-').Select(hex => Convert.ToByte(hex, 16)).ToArray();
    }

    /// <summary>
    /// Get all unique command names from the test data
    /// </summary>
    public static string[] GetAllCommandNames()
    {
        var allTestData = LoadAllTestData();
        return allTestData.Select(tc => tc.Name).Distinct().ToArray();
    }
}
