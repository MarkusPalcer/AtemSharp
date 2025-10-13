using AtemSharp.Commands.MixEffects.Key;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

[TestFixture]
public class MixEffectKeyLumaUpdateCommandTests : DeserializedCommandTestBase<MixEffectKeyLumaUpdateCommand,
    MixEffectKeyLumaUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int MixEffectIndex { get; set; }
        public int KeyerIndex { get; set; }
        public bool PreMultiplied { get; set; }
        public double Clip { get; set; }
        public double Gain { get; set; }
        public bool Invert { get; set; }
    }

    protected override void CompareCommandProperties(MixEffectKeyLumaUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
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

        // Compare PreMultiplied
        if (!actualCommand.PreMultiplied.Equals(expectedData.PreMultiplied))
        {
            failures.Add($"PreMultiplied: expected {expectedData.PreMultiplied}, actual {actualCommand.PreMultiplied}");
        }

        // Compare Clip - it is a floating point value so we approximate (1 decimal place for scaled values)
        if (!Utilities.AreApproximatelyEqual(actualCommand.Clip, expectedData.Clip, 1))
        {
            failures.Add($"Clip: expected {expectedData.Clip}, actual {actualCommand.Clip}");
        }

        // Compare Gain - it is a floating point value so we approximate (1 decimal place for scaled values)
        if (!Utilities.AreApproximatelyEqual(actualCommand.Gain, expectedData.Gain, 1))
        {
            failures.Add($"Gain: expected {expectedData.Gain}, actual {actualCommand.Gain}");
        }

        // Compare Invert
        if (!actualCommand.Invert.Equals(expectedData.Invert))
        {
            failures.Add($"Invert: expected {expectedData.Invert}, actual {actualCommand.Invert}");
        }

        // Assert results
        if (failures.Count > 0)
        {
            Assert.Fail($"Command deserialization property mismatch for version {testCase.FirstVersion}:\n" +
                       string.Join("\n", failures));
        }
    }
}