using FairlightMixerSourceExpanderUpdateCommand = AtemSharp.Commands.Fairlight.Source.FairlightMixerSourceExpanderUpdateCommand;

namespace AtemSharp.Tests.Commands.Fairlight.Source;

public class FairlightMixerSourceExpanderUpdateCommandTests : DeserializedCommandTestBase<FairlightMixerSourceExpanderUpdateCommand, FairlightMixerSourceExpanderUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort Index { get; set; }
        public long SourceId { get; set; }
        public bool ExpanderEnabled { get; set; }
        public bool GateEnabled { get; set; }
        public double Threshold { get; set; }
        public double Range { get; set; }
        public double Ratio { get; set; }
        public double Attack { get; set; }
        public double Hold { get; set; }
        public double Release { get; set; }
    }

    protected override void CompareCommandProperties(FairlightMixerSourceExpanderUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.InputId, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.SourceId, Is.EqualTo(expectedData.SourceId));
        Assert.That(actualCommand.ExpanderEnabled, Is.EqualTo(expectedData.ExpanderEnabled));
        Assert.That(actualCommand.GateEnabled, Is.EqualTo(expectedData.GateEnabled));
        Assert.That(actualCommand.Threshold, Is.EqualTo(expectedData.Threshold).Within(0.01));
        Assert.That(actualCommand.Range, Is.EqualTo(expectedData.Range).Within(0.01));
        Assert.That(actualCommand.Ratio, Is.EqualTo(expectedData.Ratio).Within(0.01));
        Assert.That(actualCommand.Attack, Is.EqualTo(expectedData.Attack).Within(0.01));
        Assert.That(actualCommand.Hold, Is.EqualTo(expectedData.Hold).Within(0.01));
        Assert.That(actualCommand.Release, Is.EqualTo(expectedData.Release).Within(0.01));
    }
}
