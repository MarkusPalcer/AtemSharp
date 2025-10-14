using AtemSharp.Commands.MixEffects.Key;
using AtemSharp.Enums;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

[TestFixture]
public class MixEffectKeyFlyPropertiesGetCommandTests : DeserializedCommandTestBase<MixEffectKeyFlyPropertiesGetCommand,
    MixEffectKeyFlyPropertiesGetCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int MixEffectIndex { get; set; }
        public int KeyerIndex { get; set; }
        public bool IsASet { get; set; }
        public bool IsBSet { get; set; }
        public int RunningToKeyFrame { get; set; }
        public int RunningToInfinite { get; set; }
    }

    protected override void CompareCommandProperties(MixEffectKeyFlyPropertiesGetCommand actualCommand, CommandData expectedData, TestCaseData testCase)
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

        // Compare IsASet
        if (!actualCommand.IsASet.Equals(expectedData.IsASet))
        {
            failures.Add($"IsASet: expected {expectedData.IsASet}, actual {actualCommand.IsASet}");
        }

        // Compare IsBSet
        if (!actualCommand.IsBSet.Equals(expectedData.IsBSet))
        {
            failures.Add($"IsBSet: expected {expectedData.IsBSet}, actual {actualCommand.IsBSet}");
        }

        // Compare RunningToKeyFrame (cast to enum)
        if (!actualCommand.IsAtKeyFrame.Equals((IsAtKeyFrame)expectedData.RunningToKeyFrame))
        {
            failures.Add($"RunningToKeyFrame: expected {(IsAtKeyFrame)expectedData.RunningToKeyFrame}, actual {actualCommand.IsAtKeyFrame}");
        }

        // Compare RunningToInfinite
        if (!actualCommand.RunToInfiniteIndex.Equals(expectedData.RunningToInfinite))
        {
            failures.Add($"RunningToInfinite: expected {expectedData.RunningToInfinite}, actual {actualCommand.RunToInfiniteIndex}");
        }

        // Assert results
        if (failures.Count > 0)
        {
            Assert.Fail($"Command deserialization property mismatch for version {testCase.FirstVersion}:\n" +
                       string.Join("\n", failures));
        }
    }
}