using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.State.Video.MixEffect;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
public class TransitionWipeCommandTests : SerializedCommandTestBase<TransitionWipeCommand,
    TransitionWipeCommandTests.CommandData>
{
    /// <inheritdoc />
    protected override Range[] GetFloatingPointByteRanges()
    {
        return
        [
            6..8, // BorderWidth
            10..12, // Symmetry
            12..14, // BorderSoftness
            14..16, // XPosition
            16..18 // YPosition
        ];
    }

    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public byte Rate { get; set; }
        public byte Pattern { get; set; }
        public double BorderWidth { get; set; }
        public ushort BorderInput { get; set; }
        public double Symmetry { get; set; }
        public double BorderSoftness { get; set; }
        public double XPosition { get; set; }
        public double YPosition { get; set; }
        public bool ReverseDirection { get; set; }
        public bool FlipFlop { get; set; }
    }

    protected override TransitionWipeCommand CreateSut(TestCaseData testCase)
    {
        return new TransitionWipeCommand(new MixEffect
        {
            Id = testCase.Command.Index,
            TransitionSettings =
            {
                Wipe =
                {
                    Rate = testCase.Command.Rate,
                    Pattern = testCase.Command.Pattern,
                    BorderWidth = testCase.Command.BorderWidth,
                    BorderInput = testCase.Command.BorderInput,
                    Symmetry = testCase.Command.Symmetry,
                    BorderSoftness = testCase.Command.BorderSoftness,
                    XPosition = testCase.Command.XPosition,
                    YPosition = testCase.Command.YPosition,
                    ReverseDirection = testCase.Command.ReverseDirection,
                    FlipFlop = testCase.Command.FlipFlop
                }
            }
        });
    }

    [Test]
    public void PropertySetters_SetCorrectFlags()
    {
        // Arrange
        var state = new MixEffect();
        var command = new TransitionWipeCommand(state);
        Assert.That(command.Flag, Is.EqualTo(0)); // Initial flags should be 0

        // Act & Assert - Test each property setter sets the correct flag
        command.Rate = 75;
        Assert.That(command.Flag & (1 << 0), Is.Not.EqualTo(0), "Rate flag should be set");

        command.Pattern = 3;
        Assert.That(command.Flag & (1 << 1), Is.Not.EqualTo(0), "Pattern flag should be set");

        command.BorderWidth = 15.5;
        Assert.That(command.Flag & (1 << 2), Is.Not.EqualTo(0), "BorderWidth flag should be set");

        command.BorderInput = 1050;
        Assert.That(command.Flag & (1 << 3), Is.Not.EqualTo(0), "BorderInput flag should be set");

        command.Symmetry = 50.0;
        Assert.That(command.Flag & (1 << 4), Is.Not.EqualTo(0), "Symmetry flag should be set");

        command.BorderSoftness = 25.7;
        Assert.That(command.Flag & (1 << 5), Is.Not.EqualTo(0), "BorderSoftness flag should be set");

        command.XPosition = 0.75;
        Assert.That(command.Flag & (1 << 6), Is.Not.EqualTo(0), "XPosition flag should be set");

        command.YPosition = 0.25;
        Assert.That(command.Flag & (1 << 7), Is.Not.EqualTo(0), "YPosition flag should be set");

        command.ReverseDirection = true;
        Assert.That(command.Flag & (1 << 8), Is.Not.EqualTo(0), "ReverseDirection flag should be set");

        command.FlipFlop = true;
        Assert.That(command.Flag & (1 << 9), Is.Not.EqualTo(0), "FlipFlop flag should be set");
    }
}
