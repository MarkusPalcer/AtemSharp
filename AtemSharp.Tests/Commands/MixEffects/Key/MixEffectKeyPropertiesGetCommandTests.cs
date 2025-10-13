using AtemSharp.Commands.MixEffects.Key;
using AtemSharp.Enums;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

[TestFixture]
public class MixEffectKeyPropertiesGetCommandTests : DeserializedCommandTestBase<MixEffectKeyPropertiesGetCommand,
    MixEffectKeyPropertiesGetCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int MixEffectIndex { get; set; }
        public int KeyerIndex { get; set; }
        public byte KeyType { get; set; }
        public bool CanFlyKey { get; set; }
        public bool FlyEnabled { get; set; }
        public int FillSource { get; set; }
        public int CutSource { get; set; }
        public bool MaskEnabled { get; set; }
        public double MaskTop { get; set; }
        public double MaskBottom { get; set; }
        public double MaskLeft { get; set; }
        public double MaskRight { get; set; }
    }

    protected override void CompareCommandProperties(MixEffectKeyPropertiesGetCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        var failures = new List<string>();

        // Compare MixEffectIndex
        if (!actualCommand.MixEffectIndex.Equals(expectedData.MixEffectIndex))
        {
            failures.Add($"MixEffectIndex: expected {expectedData.MixEffectIndex}, actual {actualCommand.MixEffectIndex}");
        }

        // Compare KeyerIndex
        if (!actualCommand.KeyerIndex.Equals(expectedData.KeyerIndex))
        {
            failures.Add($"KeyerIndex: expected {expectedData.KeyerIndex}, actual {actualCommand.KeyerIndex}");
        }

        // Compare KeyType
        if (!actualCommand.KeyType.Equals((MixEffectKeyType)expectedData.KeyType))
        {
            failures.Add($"KeyType: expected {(MixEffectKeyType)expectedData.KeyType}, actual {actualCommand.KeyType}");
        }

        // Compare CanFlyKey
        if (!actualCommand.CanFlyKey.Equals(expectedData.CanFlyKey))
        {
            failures.Add($"CanFlyKey: expected {expectedData.CanFlyKey}, actual {actualCommand.CanFlyKey}");
        }

        // Compare FlyEnabled
        if (!actualCommand.FlyEnabled.Equals(expectedData.FlyEnabled))
        {
            failures.Add($"FlyEnabled: expected {expectedData.FlyEnabled}, actual {actualCommand.FlyEnabled}");
        }

        // Compare FillSource
        if (!actualCommand.FillSource.Equals(expectedData.FillSource))
        {
            failures.Add($"FillSource: expected {expectedData.FillSource}, actual {actualCommand.FillSource}");
        }

        // Compare CutSource
        if (!actualCommand.CutSource.Equals(expectedData.CutSource))
        {
            failures.Add($"CutSource: expected {expectedData.CutSource}, actual {actualCommand.CutSource}");
        }

        // Compare MaskEnabled
        if (!actualCommand.MaskEnabled.Equals(expectedData.MaskEnabled))
        {
            failures.Add($"MaskEnabled: expected {expectedData.MaskEnabled}, actual {actualCommand.MaskEnabled}");
        }

        // Compare MaskTop (floating point - use tolerance)
        if (!Utilities.AreApproximatelyEqual(actualCommand.MaskTop, expectedData.MaskTop, 3))
        {
            failures.Add($"MaskTop: expected {expectedData.MaskTop}, actual {actualCommand.MaskTop}");
        }

        // Compare MaskBottom (floating point - use tolerance)
        if (!Utilities.AreApproximatelyEqual(actualCommand.MaskBottom, expectedData.MaskBottom, 3))
        {
            failures.Add($"MaskBottom: expected {expectedData.MaskBottom}, actual {actualCommand.MaskBottom}");
        }

        // Compare MaskLeft (floating point - use tolerance)
        if (!Utilities.AreApproximatelyEqual(actualCommand.MaskLeft, expectedData.MaskLeft, 3))
        {
            failures.Add($"MaskLeft: expected {expectedData.MaskLeft}, actual {actualCommand.MaskLeft}");
        }

        // Compare MaskRight (floating point - use tolerance)
        if (!Utilities.AreApproximatelyEqual(actualCommand.MaskRight, expectedData.MaskRight, 3))
        {
            failures.Add($"MaskRight: expected {expectedData.MaskRight}, actual {actualCommand.MaskRight}");
        }

        // Assert results
        if (failures.Count > 0)
        {
            Assert.Fail($"Command deserialization property mismatch for version {testCase.FirstVersion}:\n" +
                       string.Join("\n", failures));
        }
    }
}