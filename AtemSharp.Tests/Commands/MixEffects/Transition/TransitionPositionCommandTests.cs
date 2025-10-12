using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
public class TransitionPositionCommandTests : SerializedCommandTestBase<TransitionPositionCommand,
    TransitionPositionCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public double HandlePosition { get; set; }
    }

    protected override TransitionPositionCommand CreateSut(TestCaseData testCase)
    {
        // Create state with the required mix effect and transition position
        var state = CreateStateWithMixEffect(testCase.Command.Index, 
            testCase.Command.HandlePosition); // Use the actual handle position from test data
        
        // Create command with the mix effect ID
        var command = new TransitionPositionCommand(testCase.Command.Index, state);

        // Set the actual value that should be written
        command.HandlePosition = testCase.Command.HandlePosition;
        
        return command;
    }

    /// <summary>
    /// Creates an AtemState with a valid mix effect and transition position at the specified index
    /// </summary>
    private static AtemState CreateStateWithMixEffect(int mixEffectId, double handlePosition = 0.0)
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
                HandlePosition = handlePosition
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
        const double expectedHandlePosition = 0.5;
        var state = CreateStateWithMixEffect(mixEffectId, expectedHandlePosition);

        // Act
        var command = new TransitionPositionCommand(mixEffectId, state);

        // Assert
        Assert.That(command.MixEffectId, Is.EqualTo(mixEffectId));
        Assert.That(command.HandlePosition, Is.EqualTo(expectedHandlePosition));
        Assert.That(command.Flag, Is.EqualTo(0)); // No flags set when initialized from state
    }

    [Test]
    public void Constructor_WithMissingMixEffect_InitializesDefaults()
    {
        // Arrange
        const int mixEffectId = 1;
        var state = CreateStateWithMixEffect(0); // Only has mix effect 0, but we're asking for 1

        // Act
        var command = new TransitionPositionCommand(mixEffectId, state);

        // Assert
        Assert.That(command.MixEffectId, Is.EqualTo(mixEffectId));
        Assert.That(command.HandlePosition, Is.EqualTo(0.0));
        Assert.That(command.Flag, Is.EqualTo(1)); // Flag set because default was applied
    }

    [Test]
    public void HandlePosition_Setter_SetsFlag()
    {
        // Arrange
        var state = CreateStateWithMixEffect(0);
        var command = new TransitionPositionCommand(0, state);
        
        // Act
        command.HandlePosition = 0.75;

        // Assert
        Assert.That(command.HandlePosition, Is.EqualTo(0.75));
        Assert.That(command.Flag, Is.EqualTo(1)); // Bit 0 set
    }

    [Test]
    public void Serialize_ProducesCorrectByteArray()
    {
        // Arrange
        var state = CreateStateWithMixEffect(0);
        var command = new TransitionPositionCommand(0, state);
        command.HandlePosition = 0.5; // 50%

        // Act
        var result = command.Serialize(ProtocolVersion.V8_1_1);

        // Assert
        Assert.That(result.Length, Is.EqualTo(4));
        Assert.That(result[0], Is.EqualTo(0)); // MixEffectId
        Assert.That(result[1], Is.EqualTo(0)); // Padding
        Assert.That(result[2], Is.EqualTo(0x13)); // HandlePosition high byte (5000 = 0x1388)
        Assert.That(result[3], Is.EqualTo(0x88)); // HandlePosition low byte
    }
}