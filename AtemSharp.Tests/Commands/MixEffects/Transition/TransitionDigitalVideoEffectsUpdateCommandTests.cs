using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.Enums;
using AtemSharp.State;

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

    [Test]
    public void ApplyToState_WithValidState_UpdatesDVESettings()
    {
        // Arrange
        var command = new TransitionDigitalVideoEffectsUpdateCommand
        {
            MixEffectId = 0,
            Rate = 30,
            LogoRate = 20,
            Style = DigitalVideoEffect.SwooshBottom,
            FillSource = 1500,
            KeySource = 2500,
            EnableKey = true,
            PreMultiplied = false,
            Clip = 600,
            Gain = 800,
            InvertKey = true,
            Reverse = false,
            FlipFlop = true
        };

        var state = new AtemState
        {
            Info = new DeviceInfo
            {
                Capabilities = new AtemCapabilities
                {
                    MixEffects = 1,
                    DVEs = 1
                }
            },
            Video = new VideoState
            {
                MixEffects =
                [
                    new MixEffect
                    {
                        Id = 0,
                    }
                ]
            }
        };

        // Act
        command.ApplyToState(state);

        // Assert
        var dveSettings = state.Video.MixEffects[0].TransitionSettings.DigitalVideoEffect;
        Assert.That(dveSettings, Is.Not.Null);
        Assert.That(dveSettings.Rate, Is.EqualTo(30));
        Assert.That(dveSettings.LogoRate, Is.EqualTo(20));
        Assert.That(dveSettings.Style, Is.EqualTo(DigitalVideoEffect.SwooshBottom));
        Assert.That(dveSettings.FillSource, Is.EqualTo(1500));
        Assert.That(dveSettings.KeySource, Is.EqualTo(2500));
        Assert.That(dveSettings.EnableKey, Is.True);
        Assert.That(dveSettings.PreMultiplied, Is.False);
        Assert.That(dveSettings.Clip, Is.EqualTo(600));
        Assert.That(dveSettings.Gain, Is.EqualTo(800));
        Assert.That(dveSettings.InvertKey, Is.True);
        Assert.That(dveSettings.Reverse, Is.False);
        Assert.That(dveSettings.FlipFlop, Is.True);
    }
}
