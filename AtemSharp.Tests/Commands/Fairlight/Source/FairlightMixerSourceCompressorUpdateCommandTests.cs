using FairlightMixerSourceCompressorUpdateCommand = AtemSharp.Commands.Audio.Fairlight.Source.FairlightMixerSourceCompressorUpdateCommand;

namespace AtemSharp.Tests.Commands.Fairlight.Source;

public class FairlightMixerSourceCompressorUpdateCommandTests : DeserializedCommandTestBase<FairlightMixerSourceCompressorUpdateCommand, FairlightMixerSourceCompressorUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int Index { get; set; }
        public long SourceId { get; set; }
        public bool CompressorEnabled { get; set; }
        public double Threshold { get; set; }
        public double Ratio { get; set; }
        public double Attack { get; set; }
        public double Hold { get; set; }
        public double Release { get; set; }
    }

    protected override void CompareCommandProperties(FairlightMixerSourceCompressorUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.InputId, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.SourceId, Is.EqualTo(expectedData.SourceId));
        Assert.That(actualCommand.CompressorEnabled, Is.EqualTo(expectedData.CompressorEnabled));
        Assert.That(actualCommand.Threshold, Is.EqualTo(expectedData.Threshold).Within(0.01));
        Assert.That(actualCommand.Ratio, Is.EqualTo(expectedData.Ratio).Within(0.01));
        Assert.That(actualCommand.Attack, Is.EqualTo(expectedData.Attack).Within(0.01));
        Assert.That(actualCommand.Hold, Is.EqualTo(expectedData.Hold).Within(0.01));
        Assert.That(actualCommand.Release, Is.EqualTo(expectedData.Release).Within(0.01));
    }
}
