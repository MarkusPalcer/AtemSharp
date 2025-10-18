using AtemSharp.Commands.Fairlight;

namespace AtemSharp.Tests.Commands.Fairlight;

public class FairlightMixerMasterCompressorUpdateCommandTests : DeserializedCommandTestBase<FairlightMixerMasterCompressorUpdateCommand, FairlightMixerMasterCompressorUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public bool CompressorEnabled { get; set; }
        public double Threshold { get; set; }
        public double Ratio { get; set; }
        public double Attack { get; set; }
        public double Hold { get; set; }
        public double Release { get; set; }
    }

    protected override void CompareCommandProperties(FairlightMixerMasterCompressorUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.Parameters.CompressorEnabled, Is.EqualTo(expectedData.CompressorEnabled), $"{testCase.Name} - CompressorEnabled");
        Assert.That(actualCommand.Parameters.Threshold, Is.EqualTo(expectedData.Threshold).Within(0.01), $"{testCase.Name} - Threshold");
        Assert.That(actualCommand.Parameters.Ratio, Is.EqualTo(expectedData.Ratio).Within(0.01), $"{testCase.Name} - Ratio");
        Assert.That(actualCommand.Parameters.Attack, Is.EqualTo(expectedData.Attack).Within(0.01), $"{testCase.Name} - Attack");
        Assert.That(actualCommand.Parameters.Hold, Is.EqualTo(expectedData.Hold).Within(0.01), $"{testCase.Name} - Hold");
        Assert.That(actualCommand.Parameters.Release, Is.EqualTo(expectedData.Release).Within(0.01), $"{testCase.Name} - Release");
    }
}
