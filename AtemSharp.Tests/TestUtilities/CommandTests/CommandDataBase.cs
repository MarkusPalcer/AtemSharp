using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AtemSharp.Tests.TestUtilities.CommandTests;

/// <summary>
/// Base class for test data
/// </summary>
public abstract class CommandDataBase
{
    [JsonExtensionData] public Dictionary<string, JToken> UnknownProperties { get; set; } = new();
}
