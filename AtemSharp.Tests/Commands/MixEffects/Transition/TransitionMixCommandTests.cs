using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
public class TransitionMixCommandTests : SerializedCommandTestBase<TransitionMixCommand,
    TransitionMixCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public int Rate { get; set; }
    }

    protected override TransitionMixCommand CreateSut(TestCaseData testCase)
    {
        // Create state with the required mix effect and transition settings
        var state = CreateStateWithMixEffect(testCase.Command.Index, 
            testCase.Command.Rate); // Use the rate from test data
        
        // Create command with the mix effect ID
        var command = new TransitionMixCommand(testCase.Command.Index, state);

        // Set the actual value that should be written
        command.Rate = testCase.Command.Rate;
        
        return command;
    }

    /// <summary>
    /// Creates an AtemState with a valid mix effect and transition settings at the specified index
    /// </summary>
    private static AtemState CreateStateWithMixEffect(int mixEffectId, int rate = 25)
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
                Mix = new MixTransitionSettings
                {
                    Rate = rate
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
        const int mixEffectId = 0;
        const int expectedRate = 50;
        var state = CreateStateWithMixEffect(mixEffectId, expectedRate);

        // Act
        var command = new TransitionMixCommand(mixEffectId, state);

        // Assert
        Assert.That(command.MixEffectId, Is.EqualTo(mixEffectId));
        Assert.That(command.Rate, Is.EqualTo(expectedRate));
        Assert.That(command.Flag, Is.EqualTo(0)); // No flags set when initializing from state
    }

    [Test]
    public void Constructor_WithMissingTransitionSettings_UsesDefaults()
    {
        // Arrange
        const int mixEffectId = 1;
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
                MixEffects = new Dictionary<int, MixEffect>
                {
                    [mixEffectId] = new MixEffect
                    {
                        Index = mixEffectId,
                        TransitionSettings = null // Missing transition settings
                    }
                }
            }
        };

        // Act
        var command = new TransitionMixCommand(mixEffectId, state);

        // Assert
        Assert.That(command.MixEffectId, Is.EqualTo(mixEffectId));
        Assert.That(command.Rate, Is.EqualTo(25)); // Default value
        Assert.That(command.Flag, Is.EqualTo(1)); // Flag should be set for default value
    }

    [Test]
    public void Rate_SetProperty_SetsFlag()
    {
        // Arrange
        const int mixEffectId = 0;
        var state = CreateStateWithMixEffect(mixEffectId);
        var command = new TransitionMixCommand(mixEffectId, state);
        
        // Act
        command.Rate = 100;

        // Assert
        Assert.That(command.Rate, Is.EqualTo(100));
        Assert.That(command.Flag, Is.EqualTo(1)); // Flag should be set
    }

    [Test]
    public void Serialize_BasicData_ProducesCorrectBytes()
    {
        // Arrange
        const int mixEffectId = 2;
        const int rate = 75;
        var state = CreateStateWithMixEffect(mixEffectId);
        var command = new TransitionMixCommand(mixEffectId, state);
        command.Rate = rate;

        // Act
        var result = command.Serialize(ProtocolVersion.V8_1_1);

        // Assert
        Assert.That(result, Has.Length.EqualTo(4));
        Assert.That(result[0], Is.EqualTo(mixEffectId)); // Mix effect ID
        Assert.That(result[1], Is.EqualTo(rate)); // Rate
        Assert.That(result[2], Is.EqualTo(0)); // Padding
        Assert.That(result[3], Is.EqualTo(0)); // Padding
    }
}