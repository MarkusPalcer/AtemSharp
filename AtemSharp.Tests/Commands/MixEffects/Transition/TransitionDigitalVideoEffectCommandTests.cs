using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
// ReSharper disable once InconsistentNaming Domain Specific Acronym
public class TransitionDigitalVideoEffectCommandTests : SerializedCommandTestBase<TransitionDigitalVideoEffectCommand,
    TransitionDigitalVideoEffectCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public int Rate { get; set; }
        public int LogoRate { get; set; }
        public DigitalVideoEffect Style { get; set; }
        public int FillSource { get; set; }
        public int KeySource { get; set; }
        public bool EnableKey { get; set; }
        public bool PreMultiplied { get; set; }
        public double Clip { get; set; }  // Use double for fractional values
        public double Gain { get; set; }  // Use double for fractional values
        public bool InvertKey { get; set; }
        public bool Reverse { get; set; }
        public bool FlipFlop { get; set; }
    }

    protected override TransitionDigitalVideoEffectCommand CreateSut(TestCaseData testCase)
    {
        // Create state with the required mix effect and transition settings
        var state = new MixEffect
        {
            Index = testCase.Command.Index,
            TransitionSettings =
            {
                DigitalVideoEffect =
                {
                    Rate = testCase.Command.Rate,
                    LogoRate = testCase.Command.LogoRate,
                    Style = testCase.Command.Style,
                    FillSource = testCase.Command.FillSource,
                    KeySource = testCase.Command.KeySource,
                    EnableKey = testCase.Command.EnableKey,
                    PreMultiplied = testCase.Command.PreMultiplied,
                    Clip = testCase.Command.Clip,
                    Gain = testCase.Command.Gain,
                    InvertKey = testCase.Command.InvertKey,
                    Reverse = testCase.Command.Reverse,
                    FlipFlop = testCase.Command.FlipFlop
                }
            }
        };

        // Create command with the mix effect ID
        var command = new TransitionDigitalVideoEffectCommand(state);

        // Set the actual values that should be written
        command.Rate = testCase.Command.Rate;
        command.LogoRate = testCase.Command.LogoRate;
        command.Style = testCase.Command.Style;
        command.FillSource = testCase.Command.FillSource;
        command.KeySource = testCase.Command.KeySource;
        command.EnableKey = testCase.Command.EnableKey;
        command.PreMultiplied = testCase.Command.PreMultiplied;
        command.Clip = testCase.Command.Clip;  // Direct assignment - now both are double
        command.Gain = testCase.Command.Gain;  // Direct assignment - now both are double
        command.InvertKey = testCase.Command.InvertKey;
        command.Reverse = testCase.Command.Reverse;
        command.FlipFlop = testCase.Command.FlipFlop;

        return command;
    }

    [Test]
    public void Constructor_WithValidState_InitializesFromState()
    {
        // Arrange
        var state = new MixEffect
        {
            Index = 0,
            TransitionSettings =
            {
                DigitalVideoEffect =
                {
                    Rate = 50,
                    LogoRate = 30,
                    Style = DigitalVideoEffect.SwooshTop,
                    FillSource = 1000,
                    KeySource = 2000,
                    EnableKey = true,
                    PreMultiplied = true,
                    Clip = 500,
                    Gain = 750,
                    InvertKey = true,
                    Reverse = true,
                    FlipFlop = true
                }
            }
        };

        // Act
        var command = new TransitionDigitalVideoEffectCommand(state);

        // Assert
        Assert.That(command.MixEffectId, Is.EqualTo(0));
        Assert.That(command.Rate, Is.EqualTo(50));
        Assert.That(command.LogoRate, Is.EqualTo(30));
        Assert.That(command.Style, Is.EqualTo(DigitalVideoEffect.SwooshTop));
        Assert.That(command.FillSource, Is.EqualTo(1000));
        Assert.That(command.KeySource, Is.EqualTo(2000));
        Assert.That(command.EnableKey, Is.True);
        Assert.That(command.PreMultiplied, Is.True);
        Assert.That(command.Clip, Is.EqualTo(500));
        Assert.That(command.Gain, Is.EqualTo(750));
        Assert.That(command.InvertKey, Is.True);
        Assert.That(command.Reverse, Is.True);
        Assert.That(command.FlipFlop, Is.True);

        // No flags should be set since we initialized from state
        Assert.That(command.Flag, Is.EqualTo(0));
    }

    [Test]
    public void PropertySetters_SetCorrectFlags()
    {
        // Arrange
        var state = new MixEffect();
        var command = new TransitionDigitalVideoEffectCommand(state);

        // Act & Assert individual flag setting
        command.Rate = 25;
        Assert.That(command.Flag & (1 << 0), Is.Not.EqualTo(0), "Rate flag not set");

        command.LogoRate = 15;
        Assert.That(command.Flag & (1 << 1), Is.Not.EqualTo(0), "LogoRate flag not set");

        command.Style = DigitalVideoEffect.SwooshRight;
        Assert.That(command.Flag & (1 << 2), Is.Not.EqualTo(0), "Style flag not set");

        command.FillSource = 1000;
        Assert.That(command.Flag & (1 << 3), Is.Not.EqualTo(0), "FillSource flag not set");

        command.KeySource = 2000;
        Assert.That(command.Flag & (1 << 4), Is.Not.EqualTo(0), "KeySource flag not set");

        command.EnableKey = true;
        Assert.That(command.Flag & (1 << 5), Is.Not.EqualTo(0), "EnableKey flag not set");

        command.PreMultiplied = true;
        Assert.That(command.Flag & (1 << 6), Is.Not.EqualTo(0), "PreMultiplied flag not set");

        command.Clip = 500;
        Assert.That(command.Flag & (1 << 7), Is.Not.EqualTo(0), "Clip flag not set");

        command.Gain = 750;
        Assert.That(command.Flag & (1 << 8), Is.Not.EqualTo(0), "Gain flag not set");

        command.InvertKey = true;
        Assert.That(command.Flag & (1 << 9), Is.Not.EqualTo(0), "InvertKey flag not set");

        command.Reverse = true;
        Assert.That(command.Flag & (1 << 10), Is.Not.EqualTo(0), "Reverse flag not set");

        command.FlipFlop = true;
        Assert.That(command.Flag & (1 << 11), Is.Not.EqualTo(0), "FlipFlop flag not set");
    }
}
