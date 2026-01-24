using Argon;
using AtemSharp.State.Info;

namespace AtemSharp.Tests.TestUtilities.CommandTests.RecordedTestCases;

public class RecordedTestCase
{
    public string Name { get; set; } = string.Empty;
    public JObject? Changes { get; set; }

    public JObject? CommandCreationParameters { get; set; }

    public string Payload { get; set; } = string.Empty;
    public ProtocolVersion Version { get; set; } = ProtocolVersion.Unknown;

    public static IEnumerable<RecordedTestCase> GetRecordedTestCases(string rawName)
    {
        var json = Helper.GetRessource("AtemSharp.Tests.TestData.atem-mini-iso-pro.json");
        var allCases = JsonConvert.DeserializeObject<Dictionary<string, JToken>>(json)["commands"].ToObject<Dictionary<string, RecordedTestCase[]>>() ?? [];
        return !allCases.TryGetValue(rawName, out var testCases) ? Enumerable.Empty<RecordedTestCase>() : testCases;
    }
}
