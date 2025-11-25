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
    public void Constructor_WithValidState_InitializesFromState()
    {
        // Arrange
        byte mixEffectId = 1;
        byte expectedRate = 50;
        byte expectedPattern = 5;
        var expectedBorderWidth = 25.5;
        ushort expectedBorderInput = 1042;
        var expectedSymmetry = 75.2;
        var expectedBorderSoftness = 12.8;
        var expectedXPosition = 0.6543;
        var expectedYPosition = 0.3456;
        var expectedReverseDirection = true;
        var expectedFlipFlop = false;

        var state = new MixEffect
        {
            Id = mixEffectId,
            TransitionSettings =
            {
                Wipe =
                {
                    Rate = expectedRate,
                    Pattern = expectedPattern,
                    BorderWidth = expectedBorderWidth,
                    BorderInput = expectedBorderInput,
                    Symmetry = expectedSymmetry,
                    BorderSoftness = expectedBorderSoftness,
                    XPosition = expectedXPosition,
                    YPosition = expectedYPosition,
                    ReverseDirection = expectedReverseDirection,
                    FlipFlop = expectedFlipFlop
                }
            }
        };

        // Act
        var command = new TransitionWipeCommand(state);

        Assert.Multiple(() =>
        {
            // Assert - Verify that values are correctly initialized from state
            Assert.That(command.MixEffectId, Is.EqualTo(mixEffectId));
            Assert.That(command.Rate, Is.EqualTo(expectedRate));
            Assert.That(command.Pattern, Is.EqualTo(expectedPattern));
            Assert.That(command.BorderWidth, Is.EqualTo(expectedBorderWidth));
            Assert.That(command.BorderInput, Is.EqualTo(expectedBorderInput));
            Assert.That(command.Symmetry, Is.EqualTo(expectedSymmetry));
            Assert.That(command.BorderSoftness, Is.EqualTo(expectedBorderSoftness));
            Assert.That(command.XPosition, Is.EqualTo(expectedXPosition));
            Assert.That(command.YPosition, Is.EqualTo(expectedYPosition));
            Assert.That(command.ReverseDirection, Is.EqualTo(expectedReverseDirection));
            Assert.That(command.FlipFlop, Is.EqualTo(expectedFlipFlop));

            // Verify no flags are set when initializing from state
            Assert.That(command.Flag, Is.EqualTo(0));
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
