using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
public class PreviewTransitionCommandTests : SerializedCommandTestBase<PreviewTransitionCommand,
    PreviewTransitionCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int Index { get; set; }
        public bool PreviewTransition { get; set; }
    }

    protected override PreviewTransitionCommand CreateSut(TestCaseData testCase)
    {
        // Create state with the required mix effect
        var state = CreateStateWithMixEffect(testCase.Command.Index, testCase.Command.PreviewTransition);
        
        // Create command with the mix effect ID
        var command = new PreviewTransitionCommand(testCase.Command.Index, state);

        // Set the actual preview value that should be written
        command.Preview = testCase.Command.PreviewTransition;
        
        return command;
    }

    /// <summary>
    /// Creates an AtemState with a valid mix effect at the specified index
    /// </summary>
    private static AtemState CreateStateWithMixEffect(int mixEffectId, bool transitionPreview = false)
    {
        Dictionary<int, MixEffect> mixEffects = new Dictionary<int, MixEffect>();
        mixEffects[mixEffectId] = new MixEffect
        {
            Index = mixEffectId,
            ProgramInput = 1000,
            PreviewInput = 2000,
            TransitionPreview = transitionPreview,
            TransitionPosition = new TransitionPosition
            {
                InTransition = false,
                HandlePosition = 0,
                RemainingFrames = 0
            },
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
        const bool expectedPreview = true;
        var state = CreateStateWithMixEffect(mixEffectId, expectedPreview);

        // Act
        var command = new PreviewTransitionCommand(mixEffectId, state);

        // Assert
        Assert.That(command.MixEffectId, Is.EqualTo(mixEffectId));
        Assert.That(command.Preview, Is.EqualTo(expectedPreview));
        Assert.That(command.Flag, Is.EqualTo(0), "Flag should not be set when initializing from state");
    }

    [Test]
    public void Constructor_WithoutMixEffect_InitializesWithDefaults()
    {
        // Arrange
        const int mixEffectId = 1;
        var state = new AtemState(); // Empty state

        // Act
        var command = new PreviewTransitionCommand(mixEffectId, state);

        // Assert
        Assert.That(command.MixEffectId, Is.EqualTo(mixEffectId));
        Assert.That(command.Preview, Is.EqualTo(false));
        Assert.That(command.Flag, Is.EqualTo(1), "Flag should be set when initializing with defaults");
    }

    [Test]
    public void Preview_WhenSet_UpdatesFlagAutomatically()
    {
        // Arrange
        const int mixEffectId = 0;
        var state = CreateStateWithMixEffect(mixEffectId);
        var command = new PreviewTransitionCommand(mixEffectId, state);
        
        // Reset flag after constructor
        command.Flag = 0;

        // Act
        command.Preview = true;

        // Assert
        Assert.That(command.Preview, Is.EqualTo(true));
        Assert.That(command.Flag, Is.EqualTo(1), "Flag should be set when Preview property is changed");
    }
}