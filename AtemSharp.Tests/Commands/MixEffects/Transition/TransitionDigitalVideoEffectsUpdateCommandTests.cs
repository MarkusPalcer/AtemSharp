using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.State.Video.MixEffect.Transition;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
public class TransitionDigitalVideoEffectsUpdateCommandTests : DeserializedCommandTestBase<TransitionDigitalVideoEffectsUpdateCommand,
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

    protected override void CompareCommandProperties(TransitionDigitalVideoEffectsUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
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
}
