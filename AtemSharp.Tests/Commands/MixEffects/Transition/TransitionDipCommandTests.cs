using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
public class TransitionDipCommandTests : SerializedCommandTestBase<TransitionDipCommand,
    TransitionDipCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public int Rate { get; set; }
        public int Input { get; set; }
    }

    protected override TransitionDipCommand CreateSut(TestCaseData testCase)
    {
        // Create state with the required mix effect and transition settings
        var state = CreateStateWithMixEffect(testCase.Command.Index, 
            testCase.Command.Rate, testCase.Command.Input);
        
        // Create command with the mix effect ID
        var command = new TransitionDipCommand(testCase.Command.Index, state);

        // Set the actual values that should be written
        command.Rate = testCase.Command.Rate;
        command.Input = testCase.Command.Input;
        
        return command;
    }

    /// <summary>
    /// Creates an AtemState with a valid mix effect and dip transition settings at the specified index
    /// </summary>
    private static AtemState CreateStateWithMixEffect(int mixEffectId, int rate = 25, int input = 0)
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
                Dip = new DipTransitionSettings
                {
                    Rate = rate,
                    Input = input
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
        const int expectedInput = 1234;
        var state = CreateStateWithMixEffect(mixEffectId, expectedRate, expectedInput);

        // Act
        var command = new TransitionDipCommand(mixEffectId, state);

        // Assert
        Assert.That(command.MixEffectId, Is.EqualTo(mixEffectId));
        Assert.That(command.Rate, Is.EqualTo(expectedRate));
        Assert.That(command.Input, Is.EqualTo(expectedInput));
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
        var command = new TransitionDipCommand(mixEffectId, state);

        // Assert
        Assert.That(command.MixEffectId, Is.EqualTo(mixEffectId));
        Assert.That(command.Rate, Is.EqualTo(25)); // Default value
        Assert.That(command.Input, Is.EqualTo(0)); // Default value
        Assert.That(command.Flag, Is.EqualTo(3)); // Both flags should be set for default values (1 << 0 | 1 << 1)
    }

    [Test]
    public void Rate_SetProperty_SetsFlag()
    {
        // Arrange
        const int mixEffectId = 0;
        var state = CreateStateWithMixEffect(mixEffectId);
        var command = new TransitionDipCommand(mixEffectId, state);
        
        // Act
        command.Rate = 100;

        // Assert
        Assert.That(command.Rate, Is.EqualTo(100));
        Assert.That(command.Flag, Is.EqualTo(1)); // Flag should be set (1 << 0)
    }

    [Test]
    public void Input_SetProperty_SetsFlag()
    {
        // Arrange
        const int mixEffectId = 0;
        var state = CreateStateWithMixEffect(mixEffectId);
        var command = new TransitionDipCommand(mixEffectId, state);
        
        // Act
        command.Input = 5678;

        // Assert
        Assert.That(command.Input, Is.EqualTo(5678));
        Assert.That(command.Flag, Is.EqualTo(2)); // Flag should be set (1 << 1)
    }

    [Test]
    public void BothProperties_SetProperties_SetsBothFlags()
    {
        // Arrange
        const int mixEffectId = 0;
        var state = CreateStateWithMixEffect(mixEffectId);
        var command = new TransitionDipCommand(mixEffectId, state);
        
        // Act
        command.Rate = 75;
        command.Input = 9999;

        // Assert
        Assert.That(command.Rate, Is.EqualTo(75));
        Assert.That(command.Input, Is.EqualTo(9999));
        Assert.That(command.Flag, Is.EqualTo(3)); // Both flags should be set (1 << 0 | 1 << 1)
    }

    [Test]
    public void Serialize_BasicData_ProducesCorrectBytes()
    {
        // Arrange
        const int mixEffectId = 2;
        const int rate = 75;
        const int input = 1234;
        var state = CreateStateWithMixEffect(mixEffectId);
        var command = new TransitionDipCommand(mixEffectId, state);
        command.Rate = rate;
        command.Input = input;

        // Act
        var result = command.Serialize(ProtocolVersion.V8_1_1);

        // Assert
        Assert.That(result, Has.Length.EqualTo(8));
        Assert.That(result[0], Is.EqualTo(3)); // Flag = 3 (both flags set)
        Assert.That(result[1], Is.EqualTo(mixEffectId)); // Mix effect ID
        Assert.That(result[2], Is.EqualTo(rate)); // Rate
        Assert.That(result[3], Is.EqualTo(0)); // Padding
        Assert.That(result[4], Is.EqualTo((input >> 8) & 0xFF)); // Input high byte
        Assert.That(result[5], Is.EqualTo(input & 0xFF)); // Input low byte
        Assert.That(result[6], Is.EqualTo(0)); // Padding
        Assert.That(result[7], Is.EqualTo(0)); // Padding
    }
}