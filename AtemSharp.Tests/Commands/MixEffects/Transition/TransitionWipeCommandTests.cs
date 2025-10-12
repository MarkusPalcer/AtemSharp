using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
public class TransitionWipeCommandTests : SerializedCommandTestBase<TransitionWipeCommand,
    TransitionWipeCommandTests.CommandData>
{
    /// <inheritdoc />
    protected override Range[] GetFloatingPointByteRanges()
    {
        return [
            6..8,   // BorderWidth
            10..12, // Symmetry  
            12..14, // BorderSoftness
            14..16, // XPosition
            16..18  // YPosition
        ]; 
    }

    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public int Rate { get; set; }
        public int Pattern { get; set; }
        public double BorderWidth { get; set; }
        public int BorderInput { get; set; }
        public double Symmetry { get; set; }
        public double BorderSoftness { get; set; }
        public double XPosition { get; set; }
        public double YPosition { get; set; }
        public bool ReverseDirection { get; set; }
        public bool FlipFlop { get; set; }
    }

    protected override TransitionWipeCommand CreateSut(TestCaseData testCase)
    {
        // Create state with the required mix effect and transition settings
        var state = CreateStateWithMixEffect(testCase.Command.Index, 
            testCase.Command.Rate, testCase.Command.Pattern,
            testCase.Command.BorderWidth, testCase.Command.BorderInput,
            testCase.Command.Symmetry, testCase.Command.BorderSoftness,
            testCase.Command.XPosition, testCase.Command.YPosition,
            testCase.Command.ReverseDirection, testCase.Command.FlipFlop);
        
        // Create command with the mix effect ID
        var command = new TransitionWipeCommand(testCase.Command.Index, state);

        // Set the actual values that should be written
        command.Rate = testCase.Command.Rate;
        command.Pattern = testCase.Command.Pattern;
        command.BorderWidth = testCase.Command.BorderWidth;
        command.BorderInput = testCase.Command.BorderInput;
        command.Symmetry = testCase.Command.Symmetry;
        command.BorderSoftness = testCase.Command.BorderSoftness;
        command.XPosition = testCase.Command.XPosition;
        command.YPosition = testCase.Command.YPosition;
        command.ReverseDirection = testCase.Command.ReverseDirection;
        command.FlipFlop = testCase.Command.FlipFlop;
        
        return command;
    }

    /// <summary>
    /// Creates an AtemState with a valid mix effect and wipe transition settings at the specified index
    /// </summary>
    private static AtemState CreateStateWithMixEffect(int mixEffectId, int rate = 25, int pattern = 0,
        double borderWidth = 0.0, int borderInput = 0, double symmetry = 0.0, double borderSoftness = 0.0,
        double xPosition = 0.0, double yPosition = 0.0, bool reverseDirection = false, bool flipFlop = false)
    {
        Dictionary<int, MixEffect> mixEffects = new Dictionary<int, MixEffect>();
        mixEffects[mixEffectId] = new MixEffect
        {
            Index = mixEffectId,
            ProgramInput = 1000,
            PreviewInput = 1001,
            TransitionPreview = false,
            TransitionPosition = new TransitionPosition
            {
                InTransition = false,
                RemainingFrames = 0,
                HandlePosition = 0.0
            },
            TransitionSettings = new TransitionSettings
            {
                Wipe = new WipeTransitionSettings
                {
                    Rate = rate,
                    Pattern = pattern,
                    BorderWidth = borderWidth,
                    BorderInput = borderInput,
                    Symmetry = symmetry,
                    BorderSoftness = borderSoftness,
                    XPosition = xPosition,
                    YPosition = yPosition,
                    ReverseDirection = reverseDirection,
                    FlipFlop = flipFlop
                }
            }
        };
        
        return new AtemState
        {
            Info = new DeviceInfo
            {
                Capabilities = new AtemCapabilities
                {
                    MixEffects = mixEffectId + 1
                }
            },
            Video = new VideoState
            {
                MixEffects = mixEffects
            }
        };
    }

    [Test]
    public void Constructor_WithValidState_InitializesFromState()
    {
        // Arrange
        var mixEffectId = 1;
        var expectedRate = 50;
        var expectedPattern = 5;
        var expectedBorderWidth = 25.5;
        var expectedBorderInput = 1042;
        var expectedSymmetry = 75.2;
        var expectedBorderSoftness = 12.8;
        var expectedXPosition = 0.6543;
        var expectedYPosition = 0.3456;
        var expectedReverseDirection = true;
        var expectedFlipFlop = false;

        var state = CreateStateWithMixEffect(mixEffectId, expectedRate, expectedPattern,
            expectedBorderWidth, expectedBorderInput, expectedSymmetry, expectedBorderSoftness,
            expectedXPosition, expectedYPosition, expectedReverseDirection, expectedFlipFlop);

        // Act
        var command = new TransitionWipeCommand(mixEffectId, state);

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
    }

    [Test]
    public void Constructor_WithMissingState_InitializesWithDefaults()
    {
        // Arrange
        var mixEffectId = 2;
        var state = new AtemState
        {
            Info = new DeviceInfo
            {
                Capabilities = new AtemCapabilities
                {
                    MixEffects = mixEffectId + 1
                }
            },
            Video = new VideoState
            {
                MixEffects = new Dictionary<int, MixEffect>()
            }
        };

        // Act
        var command = new TransitionWipeCommand(mixEffectId, state);

        // Assert - Verify that default values are set and all flags are set
        Assert.That(command.MixEffectId, Is.EqualTo(mixEffectId));
        Assert.That(command.Rate, Is.EqualTo(25));
        Assert.That(command.Pattern, Is.EqualTo(0));
        Assert.That(command.BorderWidth, Is.EqualTo(0.0));
        Assert.That(command.BorderInput, Is.EqualTo(0));
        Assert.That(command.Symmetry, Is.EqualTo(0.0));
        Assert.That(command.BorderSoftness, Is.EqualTo(0.0));
        Assert.That(command.XPosition, Is.EqualTo(0.0));
        Assert.That(command.YPosition, Is.EqualTo(0.0));
        Assert.That(command.ReverseDirection, Is.EqualTo(false));
        Assert.That(command.FlipFlop, Is.EqualTo(false));
        
        // Verify all flags are set when initializing with defaults
        var expectedFlags = (1 << 0) | (1 << 1) | (1 << 2) | (1 << 3) | (1 << 4) | (1 << 5) | (1 << 6) | (1 << 7) | (1 << 8) | (1 << 9);
        Assert.That(command.Flag, Is.EqualTo(expectedFlags));
    }

    [Test]
    public void PropertySetters_SetCorrectFlags()
    {
        // Arrange
        var state = CreateStateWithMixEffect(0);
        var command = new TransitionWipeCommand(0, state);
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