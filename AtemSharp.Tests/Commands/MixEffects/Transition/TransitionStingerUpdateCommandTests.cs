using AtemSharp.Commands.MixEffects.Transition;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
public class TransitionStingerUpdateCommandTests : DeserializedCommandTestBase<TransitionStingerUpdateCommand,
    TransitionStingerUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public byte Source { get; set; }
        public bool PreMultipliedKey { get; set; }
        public double Clip { get; set; }
        public double Gain { get; set; }
        public bool Invert { get; set; }
        public ushort Preroll { get; set; }
        public ushort ClipDuration { get; set; }
        public ushort TriggerPoint { get; set; }
        public ushort MixRate { get; set; }
    }

    protected override void CompareCommandProperties(TransitionStingerUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.MixEffectId, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.Source, Is.EqualTo(expectedData.Source));
        Assert.That(actualCommand.PreMultipliedKey, Is.EqualTo(expectedData.PreMultipliedKey));
        Assert.That(actualCommand.Clip, Is.EqualTo(expectedData.Clip).Within(0.1));
        Assert.That(actualCommand.Gain, Is.EqualTo(expectedData.Gain).Within(0.1));
        Assert.That(actualCommand.Invert, Is.EqualTo(expectedData.Invert));
        Assert.That(actualCommand.Preroll, Is.EqualTo(expectedData.Preroll));
        Assert.That(actualCommand.ClipDuration, Is.EqualTo(expectedData.ClipDuration));
        Assert.That(actualCommand.TriggerPoint, Is.EqualTo(expectedData.TriggerPoint));
        Assert.That(actualCommand.MixRate, Is.EqualTo(expectedData.MixRate));
    }
}
