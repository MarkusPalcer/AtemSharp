using System.Reflection;
using AtemSharp.Attributes;
using Newtonsoft.Json;

namespace AtemSharp.Tests.TestUtilities.CommandTests.LibAtemTestCases;

public static class Helper
{
    public static TestCaseData<TTestData>[] LoadTestCases<TTestData>(string commandRawName) where TTestData : CommandDataBase, new()
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
        var commandTestCases = allTestCases.Where(tc => tc.Name == commandRawName).ToArray();

        List<TestCaseData<TTestData>> testCases = new();

        foreach (var x in commandTestCases)
        {
            testCases.Add(new TestCaseData<TTestData>
            {
                Command = x.Command.ToObject<TTestData>()!,
                Payload = CommandTests.Helper.ParseHexBytes(x.Bytes).Skip(8).ToArray(),
                FirstVersion = x.FirstVersion,
                Name = x.Name,
                Json = x.Command.ToString()
            });
        }

        return testCases.ToArray();
    }

    public static IEnumerable<TestCaseData> GetTestCases<TCommand, TTestData>() where TTestData : CommandDataBase, new()
    {
        var commandAttribute = typeof(TCommand).GetCustomAttribute<CommandAttribute>();
        Assert.That(commandAttribute, Is.Not.Null, $"CommandAttribute is required on command class {typeof(TCommand).Name}");

        var rawName = commandAttribute.RawName;
        var testCases = LoadTestCases<TTestData>(rawName).ToArray();

        var minProtocolVersion = commandAttribute.MinimumVersion;
        if (minProtocolVersion is not null)
        {
            testCases = testCases.Where(x => x.FirstVersion >=  minProtocolVersion).ToArray();
        }

        var maxProtocolVersion = typeof(TTestData).GetCustomAttribute<MaxProtocolVersionAttribute>()?.MaxVersion;
        if (maxProtocolVersion is not null)
        {
            testCases = testCases.Where(x => x.FirstVersion <= maxProtocolVersion).ToArray();
        }

        foreach (var testCase in testCases)
        {
            yield return new TestCaseData(testCase)
                        .SetName($"{rawName}_{testCase.FirstVersion}")
                        .SetDescription(
                             $"Test deserialization for {rawName} with protocol version {testCase.FirstVersion}");
        }
    }
}
