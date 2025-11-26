using System.Reflection;
using AtemSharp.Attributes;
using AtemSharp.State.Info;
using AtemSharp.Tests.TestUtilities;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AtemSharp.Tests.Commands;

/// <summary>
/// Common base class for command test classes that contains shared functionality
/// for both serialized and deserialized command tests.
/// </summary>
public abstract class CommandTestBase<TTestData>
    where TTestData : CommandTestBase<TTestData>.CommandDataBase, new()
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members | ImplicitUseTargetFlags.WithInheritors)]
    public class PartialTestCaseData
    {
        public string Name { get; set; } = "";
        public ProtocolVersion FirstVersion { get; set; }
        public string Bytes { get; set; } = "";
        public required JObject Command { get; set; }
    }

    public class TestCaseData
    {
        public string Name { get; set; } = "";
        public ProtocolVersion FirstVersion { get; set; }
        public string Bytes { get; set; } = "";
        public TTestData Command { get; set; } = new();

        public string Json { get; set; } = "";
    }

    public abstract class CommandDataBase
    {
        // Base class for test data - derived classes add specific properties
        // SerializedCommandTestBase adds Mask property in its CommandDataBase

        [JsonExtensionData] public Dictionary<string, JToken> UnknownProperties { get; set; } = new();
    }

    protected static TestCaseData[] LoadTestData<TCommand>()
    {
        // Get the raw name from the CommandAttribute
        var commandAttribute = typeof(TCommand).GetCustomAttribute<CommandAttribute>();
        if (commandAttribute == null)
        {
            throw new InvalidOperationException($"Command {typeof(TCommand).Name} must have a CommandAttribute");
        }

        var rawName = commandAttribute.RawName;
        if (string.IsNullOrEmpty(rawName))
        {
            throw new InvalidOperationException($"Command {typeof(TCommand).Name} CommandAttribute must have a RawName");
        }

        var baseTestCases = LoadTestData(rawName);
        if (baseTestCases.Length == 0)
        {
            throw new InvalidOperationException($"No test cases found for command {rawName} in libatem-data.json");
        }

        // Convert to the derived TestCaseData type
        return baseTestCases.Select(tc => new TestCaseData
        {
            Name = tc.Name,
            FirstVersion = tc.FirstVersion,
            Bytes = tc.Bytes,
            Command = tc.Command,
            Json = tc.Json
        }).ToArray();
    }

    protected static IEnumerable<NUnit.Framework.TestCaseData> GetTestCases<TCommand>()
    {
        var testCases = LoadTestData<TCommand>();
        var commandAttribute = typeof(TCommand).GetCustomAttribute<CommandAttribute>();
        Assert.That(commandAttribute, Is.Not.Null, $"CommandAttribute is required on command class {typeof(TCommand).Name}");
        var minProtocolVersion = commandAttribute.MinimumVersion;
        var rawName = commandAttribute.RawName;

        Assert.That(testCases.Length, Is.GreaterThan(0),
                    $"Should have {rawName} test cases from libatem-data.json");

        var maxProtocolVersion = typeof(TTestData).GetCustomAttribute<MaxProtocolVersionAttribute>()?.MaxVersion;

        foreach (var testCase in testCases)
        {
            if (maxProtocolVersion is not null && testCase.FirstVersion > maxProtocolVersion)
            {
                continue;
            }

            if (testCase.FirstVersion < minProtocolVersion)
            {
                continue;
            }

            yield return new NUnit.Framework.TestCaseData(testCase)
                        .SetName($"{rawName}_{testCase.FirstVersion}")
                        .SetDescription(
                             $"Test deserialization for {rawName} with protocol version {testCase.FirstVersion}");
        }
    }

    private static TestCaseData[] LoadTestData(string commandRawName)
    {
        // Load the test data file from embedded resources
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "AtemSharp.Tests.TestData.libatem-data.json";

        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            throw new FileNotFoundException($"Could not find embedded resource: {resourceName}");
        }

        using var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();
        var allTestCases = JsonConvert.DeserializeObject<PartialTestCaseData[]>(json) ?? [];

        // Filter by the provided command raw name
        return allTestCases.Where(tc => tc.Name == commandRawName)
                           .Select(x => new TestCaseData
                            {
                                Command = x.Command.ToObject<TTestData>()!,
                                Bytes = x.Bytes,
                                FirstVersion = x.FirstVersion,
                                Name = x.Name,
                                Json = x.Command.ToString()
                            })
                           .ToArray();
    }

    /// <summary>
    /// Parse a hex string (e.g., "01-02-03") into a byte array
    /// </summary>
    protected static byte[] ParseHexBytes(string hexString)
    {
        return hexString.Split('-').Select(hex => Convert.ToByte(hex, 16)).ToArray();
    }

    /// <summary>
    /// Extract the command payload from a full packet by skipping the header and command name
    /// </summary>
    protected static byte[] ExtractCommandPayload(byte[] fullPacket)
    {
        // Skip packet header (4 bytes) + command name (4 bytes) = 8 bytes
        return fullPacket.Skip(8).ToArray();
    }

    /// <summary>
    /// Extract a fixed-size command payload from a full packet by skipping the header and command name
    /// </summary>
    protected static byte[] ExtractCommandPayload(byte[] fullPacket, int payloadSize)
    {
        // Skip packet header (4 bytes) + command name (4 bytes) = 8 bytes
        return fullPacket.Skip(8).Take(payloadSize).ToArray();
    }

    /// <summary>
    /// Generate NUnit test cases from the loaded test data.
    /// Subclasses can override to customize test case naming and description.
    /// </summary>
    [UsedImplicitly]
    protected static IEnumerable<NUnit.Framework.TestCaseData> GenerateTestCases<TCommand>(
        TestCaseData[] testCases, string commandDisplayName)
    {
        Assert.That(testCases.Length, Is.GreaterThan(0),
                    $"Should have {commandDisplayName} test cases from libatem-data.json");

        foreach (var testCase in testCases)
        {
            yield return new NUnit.Framework.TestCaseData(testCase)
                        .SetName(GetTestCaseName(testCase, commandDisplayName))
                        .SetDescription(GetTestCaseDescription(testCase, commandDisplayName));
        }
    }

    /// <summary>
    /// Generate the name for a test case.
    /// </summary>
    protected static string GetTestCaseName(TestCaseData testCase, string commandDisplayName)
    {
        return $"{commandDisplayName}_{testCase.FirstVersion}";
    }

    /// <summary>
    /// Generate the description for a test case.
    /// </summary>
    protected static string GetTestCaseDescription(TestCaseData testCase, string commandDisplayName)
    {
        return $"Test for {commandDisplayName} with protocol version {testCase.FirstVersion}";
    }
}
