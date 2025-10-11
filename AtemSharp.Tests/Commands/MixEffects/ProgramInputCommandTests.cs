using AtemSharp.Commands;
using AtemSharp.Commands.MixEffects;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects;

[TestFixture]
public class ProgramInputCommandTests : SerializedCommandTestBase<ProgramInputCommand,
    ProgramInputCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int Index { get; set; }
        public int Source { get; set; }
    }

    protected override ProgramInputCommand CreateSut(TestCaseData testCase)
    {
        // Create state with the required mix effect
        var state = CreateStateWithMixEffect(testCase.Command.Index, testCase.Command.Source);
        
        // Create command with the mix effect ID
        var command = new ProgramInputCommand(testCase.Command.Index, state);

        // Set the actual source value that should be written
        command.Source = testCase.Command.Source;
        
        return command;
    }

    /// <summary>
    /// Creates an AtemState with a valid mix effect at the specified index
    /// </summary>
    private static AtemState CreateStateWithMixEffect(int mixEffectId, int programInput = 0)
    {
        var mixEffects = new MixEffect?[Math.Max(mixEffectId + 1, 2)];
        mixEffects[mixEffectId] = new MixEffect
        {
            Index = mixEffectId,
            ProgramInput = programInput,
            PreviewInput = 1000,
            TransitionPreview = false,
            TransitionPosition = new TransitionPosition
            {
                InTransition = false,
                HandlePosition = 0,
                RemainingFrames = 0
            },
            TransitionProperties = new TransitionProperties(),
            TransitionSettings = new TransitionSettings(),
            UpstreamKeyers = []
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
        const int expectedSource = 1234;
        var state = CreateStateWithMixEffect(mixEffectId, expectedSource);

        // Act
        var command = new ProgramInputCommand(mixEffectId, state);

        // Assert
        Assert.That(command.MixEffectId, Is.EqualTo(mixEffectId));
        Assert.That(command.Source, Is.EqualTo(expectedSource));
        Assert.That(command.Flag, Is.EqualTo(0), "Flag should not be set when initializing from state");
    }

    [Test]
    public void Constructor_WithoutMixEffect_InitializesWithDefaults()
    {
        // Arrange
        const int mixEffectId = 1;
        var state = new AtemState(); // Empty state

        // Act
        var command = new ProgramInputCommand(mixEffectId, state);

        // Assert
        Assert.That(command.MixEffectId, Is.EqualTo(mixEffectId));
        Assert.That(command.Source, Is.EqualTo(0));
        Assert.That(command.Flag, Is.EqualTo(1), "Flag should be set when initializing with defaults");
    }

    [Test]
    public void Source_WhenSet_UpdatesFlagAutomatically()
    {
        // Arrange
        const int mixEffectId = 0;
        var state = CreateStateWithMixEffect(mixEffectId, 100);
        var command = new ProgramInputCommand(mixEffectId, state);
        
        // Reset flag after constructor
        command.Flag = 0;

        // Act
        command.Source = 2000;

        // Assert
        Assert.That(command.Source, Is.EqualTo(2000));
        Assert.That(command.Flag, Is.EqualTo(1), "Flag should be set when Source property is changed");
    }
}