using AtemSharp.State.Info;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace AtemSharp.Tests.TestUtilities.CommandTests.LibAtemTestCases;

[UsedImplicitly(ImplicitUseTargetFlags.Members | ImplicitUseTargetFlags.WithInheritors)]
public class PartialTestCaseData
{
    public string Name { get; set; } = "";
    public ProtocolVersion FirstVersion { get; set; }
    public string Bytes { get; set; } = "";
    public required JObject Command { get; set; }
}
