using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
public class TransitionPositionCommandTests : SerializedCommandTestBase<TransitionPositionCommand,
    TransitionPositionCommandTests.CommandData>
{
    protected override Range[] GetFloatingPointByteRanges() =>
    [
        (2..4) // HandlePosition
    ];

    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public double HandlePosition { get; set; }
    }

    protected override TransitionPositionCommand CreateSut(TestCaseData testCase)
    {
        // Create state with the required mix effect and transition position
        var state = new MixEffect
        {
            Index = testCase.Command.Index,
            TransitionPosition = new TransitionPosition
            {
                HandlePosition = testCase.Command.HandlePosition
            }
        };

        // Create command with the mix effect ID
        var command = new TransitionPositionCommand(state);

        // Set the actual value that should be written
        command.HandlePosition = testCase.Command.HandlePosition;

        return command;
    }

    [Test]
    public void Constructor_WithValidState_InitializesFromState()
    {
        // Arrange
        const int mixEffectId = 0;
        const double expectedHandlePosition = 0.5;
        var state = new MixEffect
        {
            Index = mixEffectId,
            TransitionPosition = new TransitionPosition
            {
                HandlePosition = expectedHandlePosition
            }
        };

        // Act
        var command = new TransitionPositionCommand(state);

        // Assert
        Assert.That(command.MixEffectId, Is.EqualTo(mixEffectId));
        Assert.That(command.HandlePosition, Is.EqualTo(expectedHandlePosition));
        Assert.That(command.Flag, Is.EqualTo(0)); // No flags set when initialized from state
    }

    [Test]
    public void HandlePosition_Setter_SetsFlag()
    {
        // Arrange
        var state = new MixEffect
        {
            Index = 0,
            TransitionPosition = new TransitionPosition
            {
                HandlePosition = 0
            }
        };
        var command = new TransitionPositionCommand(state);

        // Act
        command.HandlePosition = 0.75;

        // Assert
        Assert.That(command.HandlePosition, Is.EqualTo(0.75));
        Assert.That(command.Flag, Is.EqualTo(1)); // Bit 0 set
    }
}
