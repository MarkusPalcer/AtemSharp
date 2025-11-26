using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.State.Video.MixEffect;
using AtemSharp.State.Video.MixEffect.Transition;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
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
        public double Clip { get; set; } // Use double for fractional values
        public double Gain { get; set; } // Use double for fractional values
        public bool InvertKey { get; set; }
        public bool Reverse { get; set; }
        public bool FlipFlop { get; set; }
    }

    protected override TransitionDigitalVideoEffectCommand CreateSut(TestCaseData testCase)
    {
        return new TransitionDigitalVideoEffectCommand(new MixEffect
        {
            Id = testCase.Command.Index,
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
        });
    }

    [Test]
    public void PropertySetters_SetCorrectFlags()
    {
        // Arrange
        var command = new TransitionDigitalVideoEffectCommand(new MixEffect());

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
