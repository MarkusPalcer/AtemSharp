using FairlightMixerSourceLimiterUpdateCommand = AtemSharp.Commands.Audio.Fairlight.Source.FairlightMixerSourceLimiterUpdateCommand;

namespace AtemSharp.Tests.Commands.Fairlight.Source;

public class FairlightMixerSourceLimiterUpdateCommandTests : DeserializedCommandTestBase<FairlightMixerSourceLimiterUpdateCommand,
    FairlightMixerSourceLimiterUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort Index { get; set; }
        public long SourceId { get; set; }
        public bool LimiterEnabled { get; set; }
        public double Threshold { get; set; }
        public double Attack { get; set; }
        public double Hold { get; set; }
        public double Release { get; set; }
    }


    protected override void CompareCommandProperties(FairlightMixerSourceLimiterUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.InputId, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.SourceId, Is.EqualTo(expectedData.SourceId));
        Assert.That(actualCommand.LimiterEnabled, Is.EqualTo(expectedData.LimiterEnabled));
        Assert.That(actualCommand.Threshold, Is.EqualTo(expectedData.Threshold).Within(0.01));
        Assert.That(actualCommand.Attack, Is.EqualTo(expectedData.Attack).Within(0.01));
        Assert.That(actualCommand.Hold, Is.EqualTo(expectedData.Hold).Within(0.01));
        Assert.That(actualCommand.Release, Is.EqualTo(expectedData.Release).Within(0.01));
    }
}
