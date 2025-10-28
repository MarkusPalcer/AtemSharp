using AtemSharp.Commands.MixEffects.Transition;
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
        var state = new MixEffect()
        {
            Index = testCase.Command.Index,
            TransitionSettings =
            {
                Mix =
                {
                    Rate = 25 // Default rate
                }
            }
        };

        // Create command with the mix effect ID
        var command = new TransitionMixCommand(state);

        // Set the actual value that should be written
        command.Rate = testCase.Command.Rate;

        return command;
    }

    [Test]
    public void Constructor_WithValidState_InitializesFromState()
    {
        // Arrange
        const int mixEffectId = 0;
        const int expectedRate = 50;
        var state = new MixEffect
        {
            Index = mixEffectId,
            TransitionSettings =
            {
                Mix =
                {
                    Rate = expectedRate
                }
            }
        };

        // Act
        var command = new TransitionMixCommand(state);

        // Assert
        Assert.That(command.MixEffectId, Is.EqualTo(mixEffectId));
        Assert.That(command.Rate, Is.EqualTo(expectedRate));
        Assert.That(command.Flag, Is.EqualTo(0)); // No flags set when initializing from state
    }

    [Test]
    public void Rate_SetProperty_SetsFlag()
    {
        // Arrange
        const int mixEffectId = 0;
        var state = new MixEffect
        {
            Index = mixEffectId,
            TransitionSettings =
            {
                Mix =
                {
                    Rate = 0
                }
            }
        };

        var command = new TransitionMixCommand(state);

        // Act
        command.Rate = 100;

        // Assert
        Assert.That(command.Rate, Is.EqualTo(100));
        Assert.That(command.Flag, Is.EqualTo(1)); // Flag should be set
    }
}
