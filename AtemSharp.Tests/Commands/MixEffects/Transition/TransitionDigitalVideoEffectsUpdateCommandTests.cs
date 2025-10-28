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
        public double Clip { get; set; }  // Use double for fractional values
        public double Gain { get; set; }  // Use double for fractional values
        public bool InvertKey { get; set; }
        public bool Reverse { get; set; }
        public bool FlipFlop { get; set; }
    }

    protected override void CompareCommandProperties(TransitionDigitalVideoEffectsUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
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
        byte mixEffectId = 0;
        var command = new TransitionDigitalVideoEffectsUpdateCommand
        {
            MixEffectId = mixEffectId,
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
                MixEffects = new Dictionary<int, MixEffect>
                {
                    [mixEffectId] = new()
                    {
                        Index = mixEffectId,
                    }
                }
            }
        };

        // Act
        command.ApplyToState(state);

        // Assert
        var dveSettings = state.Video.MixEffects[mixEffectId].TransitionSettings.DigitalVideoEffect;
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

    [Test]
    public void ApplyToState_WithInvalidMixEffect_ThrowsInvalidIdError()
    {
        // Arrange
        var command = new TransitionDigitalVideoEffectsUpdateCommand { MixEffectId = 5 };
        var state = new AtemState
        {
            Video = new VideoState { MixEffects = new Dictionary<int, MixEffect>() }
        };

        // Act & Assert
        var ex = Assert.Throws<InvalidIdError>(() => command.ApplyToState(state));
        Assert.That(ex.Message, Contains.Substring("MixEffect"));
        Assert.That(ex.Message, Contains.Substring("5"));
    }

    [Test]
    public void ApplyToState_WithoutDVECapability_ThrowsInvalidIdError()
    {
        // Arrange
        var command = new TransitionDigitalVideoEffectsUpdateCommand { MixEffectId = 0 };
        var state = new AtemState
        {
            Info = new DeviceInfo
            {
                Capabilities = new AtemCapabilities
                {
                    MixEffects = 1,
                    DVEs = 0  // No DVE capability
                }
            },
            Video = new VideoState
            {
                MixEffects = new Dictionary<int, MixEffect>
                {
                    [0] = new MixEffect { Index = 0 }
                }
            }
        };

        // Act & Assert
        var ex = Assert.Throws<InvalidIdError>(() => command.ApplyToState(state));
        Assert.That(ex.Message, Contains.Substring("Invalid DVE id: is not supported"));
    }
}
