using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
public class TransitionPropertiesCommandTests : SerializedCommandTestBase<TransitionPropertiesCommand,
    TransitionPropertiesCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int Index { get; set; }
        public byte NextStyle { get; set; }
        public byte NextSelection { get; set; }
    }

    protected override TransitionPropertiesCommand CreateSut(TestCaseData testCase)
    {
        // Create state with the required mix effect and transition properties
        var state = CreateStateWithMixEffect(testCase.Command.Index, 
            (TransitionStyle)testCase.Command.NextStyle,
            (TransitionSelection)testCase.Command.NextSelection);
        
        // Create command with the mix effect ID
        var command = new TransitionPropertiesCommand(testCase.Command.Index, state);

        // Set the actual values that should be written
        command.NextStyle = (TransitionStyle)testCase.Command.NextStyle;
        command.NextSelection = (TransitionSelection)testCase.Command.NextSelection;
        
        return command;
    }

    /// <summary>
    /// Creates an AtemState with a valid mix effect and transition properties at the specified index
    /// </summary>
    private static AtemState CreateStateWithMixEffect(int mixEffectId, 
        TransitionStyle nextStyle = TransitionStyle.Mix, 
        TransitionSelection nextSelection = TransitionSelection.Background)
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
                HandlePosition = 0,
                RemainingFrames = 0
            },
            TransitionProperties = new TransitionProperties
            {
                Style = TransitionStyle.Mix,
                Selection = TransitionSelection.Background,
                NextStyle = nextStyle,
                NextSelection = nextSelection
            }
        };

        return new AtemState
        {
            Info = new DeviceInfo
            {
                Capabilities = new AtemCapabilities
                {
                    MixEffects = Math.Max(mixEffectId + 1, 2)
                }
            },
            Video = new VideoState
            {
                MixEffects = mixEffects
            }
        };
    }

    [Test]
    public void Constructor_WithValidMixEffect_InitializesFromState()
    {
        // Arrange
        const int mixEffectId = 1;
        const TransitionStyle expectedNextStyle = TransitionStyle.Wipe;
        const TransitionSelection expectedNextSelection = TransitionSelection.Key1 | TransitionSelection.Background;
        var state = CreateStateWithMixEffect(mixEffectId, expectedNextStyle, expectedNextSelection);

        // Act
        var command = new TransitionPropertiesCommand(mixEffectId, state);

        // Assert
        Assert.That(command.MixEffectId, Is.EqualTo(mixEffectId));
        Assert.That(command.NextStyle, Is.EqualTo(expectedNextStyle));
        Assert.That(command.NextSelection, Is.EqualTo(expectedNextSelection));
        Assert.That(command.Flag, Is.EqualTo(0), "Flag should not be set when initializing from state");
    }

    [Test]
    public void Constructor_WithoutTransitionProperties_InitializesWithDefaults()
    {
        // Arrange
        const int mixEffectId = 1;
        var state = new AtemState
        {
            Video = new VideoState
            {
                MixEffects = new Dictionary<int, MixEffect>
                {
                    [mixEffectId] = new MixEffect { Index = mixEffectId } // No TransitionProperties
                }
            }
        };

        // Act
        var command = new TransitionPropertiesCommand(mixEffectId, state);

        // Assert
        Assert.That(command.MixEffectId, Is.EqualTo(mixEffectId));
        Assert.That(command.NextStyle, Is.EqualTo(TransitionStyle.Mix));
        Assert.That(command.NextSelection, Is.EqualTo(TransitionSelection.Background));
        Assert.That(command.Flag, Is.EqualTo(3), "Both flags should be set when initializing with defaults");
    }

    [Test]
    public void Constructor_WithoutMixEffect_InitializesWithDefaults()
    {
        // Arrange
        const int mixEffectId = 1;
        var state = new AtemState(); // Empty state

        // Act
        var command = new TransitionPropertiesCommand(mixEffectId, state);

        // Assert
        Assert.That(command.MixEffectId, Is.EqualTo(mixEffectId));
        Assert.That(command.NextStyle, Is.EqualTo(TransitionStyle.Mix));
        Assert.That(command.NextSelection, Is.EqualTo(TransitionSelection.Background));
        Assert.That(command.Flag, Is.EqualTo(3), "Both flags should be set when initializing with defaults");
    }

    [Test]
    public void NextStyle_WhenSet_UpdatesFlagAutomatically()
    {
        // Arrange
        var state = CreateStateWithMixEffect(0);
        var command = new TransitionPropertiesCommand(0, state);
        Assert.That(command.Flag, Is.EqualTo(0), "Initial flag should be 0");

        // Act
        command.NextStyle = TransitionStyle.Dip;

        // Assert
        Assert.That(command.NextStyle, Is.EqualTo(TransitionStyle.Dip));
        Assert.That(command.Flag, Is.EqualTo(1), "Flag should have NextStyle bit set");
    }

    [Test]
    public void NextSelection_WhenSet_UpdatesFlagAutomatically()
    {
        // Arrange
        var state = CreateStateWithMixEffect(0);
        var command = new TransitionPropertiesCommand(0, state);
        Assert.That(command.Flag, Is.EqualTo(0), "Initial flag should be 0");

        // Act
        command.NextSelection = TransitionSelection.Key1 | TransitionSelection.Key2;

        // Assert
        Assert.That(command.NextSelection, Is.EqualTo(TransitionSelection.Key1 | TransitionSelection.Key2));
        Assert.That(command.Flag, Is.EqualTo(2), "Flag should have NextSelection bit set");
    }

    [Test]
    public void BothProperties_WhenSet_UpdatesFlagsCombined()
    {
        // Arrange
        var state = CreateStateWithMixEffect(0);
        var command = new TransitionPropertiesCommand(0, state);
        Assert.That(command.Flag, Is.EqualTo(0), "Initial flag should be 0");

        // Act
        command.NextStyle = TransitionStyle.DVE;
        command.NextSelection = TransitionSelection.Background | TransitionSelection.Key3;

        // Assert
        Assert.That(command.NextStyle, Is.EqualTo(TransitionStyle.DVE));
        Assert.That(command.NextSelection, Is.EqualTo(TransitionSelection.Background | TransitionSelection.Key3));
        Assert.That(command.Flag, Is.EqualTo(3), "Flag should have both bits set");
    }
}