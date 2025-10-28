using AtemSharp.Commands.MixEffects.Key;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

[TestFixture]
public class MixEffectKeyAdvancedChromaPropertiesUpdateCommandTests : DeserializedCommandTestBase<MixEffectKeyAdvancedChromaPropertiesUpdateCommand, MixEffectKeyAdvancedChromaPropertiesUpdateCommandTests.CommandData>
{

    public class CommandData : CommandDataBase
    {
        public byte MixEffectIndex { get; set; }
        public byte KeyerIndex { get; set; }
        public double ForegroundLevel { get; set; }
        public double BackgroundLevel { get; set; }
        public double KeyEdge { get; set; }
        public double SpillSuppression { get; set; }
        public double FlareSuppression { get; set; }
        public double Brightness { get; set; }
        public double Contrast { get; set; }
        public double Saturation { get; set; }
        public double Red { get; set; }
        public double Green { get; set; }
        public double Blue { get; set; }
    }

    protected override void CompareCommandProperties(MixEffectKeyAdvancedChromaPropertiesUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        var failures = new List<string>();

        // Compare MixEffectIndex - it is not floating point so it needs to equal
        if (!actualCommand.MixEffectId.Equals(expectedData.MixEffectIndex))
        {
            failures.Add($"MixEffectId: expected {expectedData.MixEffectIndex}, actual {actualCommand.MixEffectId}");
        }

        // Compare KeyerIndex - it is not floating point so it needs to equal
        if (!actualCommand.KeyerId.Equals(expectedData.KeyerIndex))
        {
            failures.Add($"KeyerId: expected {expectedData.KeyerIndex}, actual {actualCommand.KeyerId}");
        }

        // Compare floating-point properties with tolerance (1 decimal place for values scaled by 10)
        if (!Utilities.AreApproximatelyEqual(actualCommand.ForegroundLevel, expectedData.ForegroundLevel, 1))
        {
            failures.Add($"ForegroundLevel: expected {expectedData.ForegroundLevel}, actual {actualCommand.ForegroundLevel}");
        }

        if (!Utilities.AreApproximatelyEqual(actualCommand.BackgroundLevel, expectedData.BackgroundLevel, 1))
        {
            failures.Add($"BackgroundLevel: expected {expectedData.BackgroundLevel}, actual {actualCommand.BackgroundLevel}");
        }

        if (!Utilities.AreApproximatelyEqual(actualCommand.KeyEdge, expectedData.KeyEdge, 1))
        {
            failures.Add($"KeyEdge: expected {expectedData.KeyEdge}, actual {actualCommand.KeyEdge}");
        }

        if (!Utilities.AreApproximatelyEqual(actualCommand.SpillSuppression, expectedData.SpillSuppression, 1))
        {
            failures.Add($"SpillSuppression: expected {expectedData.SpillSuppression}, actual {actualCommand.SpillSuppression}");
        }

        if (!Utilities.AreApproximatelyEqual(actualCommand.FlareSuppression, expectedData.FlareSuppression, 1))
        {
            failures.Add($"FlareSuppression: expected {expectedData.FlareSuppression}, actual {actualCommand.FlareSuppression}");
        }

        if (!Utilities.AreApproximatelyEqual(actualCommand.Brightness, expectedData.Brightness, 1))
        {
            failures.Add($"Brightness: expected {expectedData.Brightness}, actual {actualCommand.Brightness}");
        }

        if (!Utilities.AreApproximatelyEqual(actualCommand.Contrast, expectedData.Contrast, 1))
        {
            failures.Add($"Contrast: expected {expectedData.Contrast}, actual {actualCommand.Contrast}");
        }

        if (!Utilities.AreApproximatelyEqual(actualCommand.Saturation, expectedData.Saturation, 1))
        {
            failures.Add($"Saturation: expected {expectedData.Saturation}, actual {actualCommand.Saturation}");
        }

        if (!Utilities.AreApproximatelyEqual(actualCommand.Red, expectedData.Red, 1))
        {
            failures.Add($"Red: expected {expectedData.Red}, actual {actualCommand.Red}");
        }

        if (!Utilities.AreApproximatelyEqual(actualCommand.Green, expectedData.Green, 1))
        {
            failures.Add($"Green: expected {expectedData.Green}, actual {actualCommand.Green}");
        }

        if (!Utilities.AreApproximatelyEqual(actualCommand.Blue, expectedData.Blue, 1))
        {
            failures.Add($"Blue: expected {expectedData.Blue}, actual {actualCommand.Blue}");
        }

        // Assert results
        if (failures.Count > 0)
        {
            Assert.Fail($"Command deserialization property mismatch for version {testCase.FirstVersion}:\n" +
                       string.Join("\n", failures));
        }
    }
}
