using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace AtemSharp.Tests.TestUtilities.CommandTests.RecordedTestCases;

[UsedImplicitly]
public class PartialTestCase
{
    /// <summary>
    /// The name of the test case
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The payload of the command
    /// </summary>
    public string Payload { get; set; } = string.Empty;

    /// <summary>
    /// The actual command data
    /// </summary>
    public JToken Command { get; set; } = new JObject();
}
