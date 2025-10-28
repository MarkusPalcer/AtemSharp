using AtemSharp.Commands.MixEffects.Key;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

[TestFixture]
public class MixEffectKeyOnAirUpdateCommandTests : DeserializedCommandTestBase<MixEffectKeyOnAirUpdateCommand,
    MixEffectKeyOnAirUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte MixEffectIndex { get; set; }
        public byte KeyerIndex { get; set; }
        public bool OnAir { get; set; }
    }

    protected override void CompareCommandProperties(MixEffectKeyOnAirUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        var failures = new List<string>();

        // Compare MixEffectId
        if (!actualCommand.MixEffectId.Equals(expectedData.MixEffectIndex))
        {
            failures.Add($"MixEffectId: expected {expectedData.MixEffectIndex}, actual {actualCommand.MixEffectId}");
        }

        // Compare KeyerId
        if (!actualCommand.KeyerId.Equals(expectedData.KeyerIndex))
        {
            failures.Add($"KeyerId: expected {expectedData.KeyerIndex}, actual {actualCommand.KeyerId}");
        }

        // Compare OnAir
        if (!actualCommand.OnAir.Equals(expectedData.OnAir))
        {
            failures.Add($"OnAir: expected {expectedData.OnAir}, actual {actualCommand.OnAir}");
        }

        // Assert results
        if (failures.Count > 0)
        {
            Assert.Fail($"Command deserialization property mismatch for version {testCase.FirstVersion}:\n" +
                       string.Join("\n", failures));
        }
    }
}
