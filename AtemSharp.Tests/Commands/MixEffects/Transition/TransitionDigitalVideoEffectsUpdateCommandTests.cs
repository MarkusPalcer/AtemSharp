using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.State;
using AtemSharp.State.Video.MixEffect.Transition;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
internal class TransitionDigitalVideoEffectsUpdateCommandTests : DeserializedCommandTestBase<TransitionDigitalVideoEffectsUpdateCommand,
    TransitionDigitalVideoEffectsUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public byte Rate { get; set; }
        public byte LogoRate { get; set; }
        public DigitalVideoEffect Style { get; set; }
        public ushort FillSource { get; set; }
        public ushort KeySource { get; set; }
        public bool EnableKey { get; set; }
        public bool PreMultiplied { get; set; }
        public double Clip { get; set; }
        public double Gain { get; set; }
        public bool InvertKey { get; set; }
        public bool Reverse { get; set; }
        public bool FlipFlop { get; set; }
    }

    internal override void CompareCommandProperties(TransitionDigitalVideoEffectsUpdateCommand actualCommand, CommandData expectedData, TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        Assert.That(actualCommand.MixEffectId, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.Rate, Is.EqualTo(expectedData.Rate));
        Assert.That(actualCommand.LogoRate, Is.EqualTo(expectedData.LogoRate));
        Assert.That(actualCommand.Style, Is.EqualTo(expectedData.Style));
        Assert.That(actualCommand.FillSource, Is.EqualTo(expectedData.FillSource));
        Assert.That(actualCommand.KeySource, Is.EqualTo(expectedData.KeySource));
        Assert.That(actualCommand.EnableKey, Is.EqualTo(expectedData.EnableKey));
        Assert.That(actualCommand.PreMultiplied, Is.EqualTo(expectedData.PreMultiplied));
        Assert.That(actualCommand.Clip, Is.EqualTo(expectedData.Clip).Within(0.1));
        Assert.That(actualCommand.Gain, Is.EqualTo(expectedData.Gain).Within(0.1));
        Assert.That(actualCommand.InvertKey, Is.EqualTo(expectedData.InvertKey));
        Assert.That(actualCommand.Reverse, Is.EqualTo(expectedData.Reverse));
        Assert.That(actualCommand.FlipFlop, Is.EqualTo(expectedData.FlipFlop));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Video.MixEffects.GetOrCreate(expectedData.Index);
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var actualCommand = state.Video.MixEffects[expectedData.Index].TransitionSettings.DigitalVideoEffect;
        Assert.That(actualCommand.Rate, Is.EqualTo(expectedData.Rate));
        Assert.That(actualCommand.LogoRate, Is.EqualTo(expectedData.LogoRate));
        Assert.That(actualCommand.Style, Is.EqualTo(expectedData.Style));
        Assert.That(actualCommand.FillSource, Is.EqualTo(expectedData.FillSource));
        Assert.That(actualCommand.KeySource, Is.EqualTo(expectedData.KeySource));
        Assert.That(actualCommand.EnableKey, Is.EqualTo(expectedData.EnableKey));
        Assert.That(actualCommand.PreMultipliedKey.Enabled, Is.EqualTo(expectedData.PreMultiplied));
        Assert.That(actualCommand.PreMultipliedKey.Clip, Is.EqualTo(expectedData.Clip).Within(0.1));
        Assert.That(actualCommand.PreMultipliedKey.Gain, Is.EqualTo(expectedData.Gain).Within(0.1));
        Assert.That(actualCommand.PreMultipliedKey.Inverted, Is.EqualTo(expectedData.InvertKey));
        Assert.That(actualCommand.Reverse, Is.EqualTo(expectedData.Reverse));
        Assert.That(actualCommand.FlipFlop, Is.EqualTo(expectedData.FlipFlop));
    }
}
