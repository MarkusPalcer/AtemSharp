using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
internal class TransitionStingerUpdateCommandTests : DeserializedCommandTestBase<TransitionStingerUpdateCommand,
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

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Video.MixEffects.GetOrCreate(expectedData.Index);
    }

    internal override void CompareCommandProperties(TransitionStingerUpdateCommand actualCommand, CommandData expectedData, TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
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

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var actualCommand = state.Video.MixEffects[expectedData.Index].TransitionSettings.Stinger;
        Assert.That(actualCommand.Source, Is.EqualTo(expectedData.Source));
        Assert.That(actualCommand.PreMultipliedKey.Enabled, Is.EqualTo(expectedData.PreMultipliedKey));
        Assert.That(actualCommand.PreMultipliedKey.Clip, Is.EqualTo(expectedData.Clip).Within(0.1));
        Assert.That(actualCommand.PreMultipliedKey.Gain, Is.EqualTo(expectedData.Gain).Within(0.1));
        Assert.That(actualCommand.PreMultipliedKey.Inverted, Is.EqualTo(expectedData.Invert));
        Assert.That(actualCommand.Preroll, Is.EqualTo(expectedData.Preroll));
        Assert.That(actualCommand.ClipDuration, Is.EqualTo(expectedData.ClipDuration));
        Assert.That(actualCommand.TriggerPoint, Is.EqualTo(expectedData.TriggerPoint));
        Assert.That(actualCommand.MixRate, Is.EqualTo(expectedData.MixRate));
    }
}
