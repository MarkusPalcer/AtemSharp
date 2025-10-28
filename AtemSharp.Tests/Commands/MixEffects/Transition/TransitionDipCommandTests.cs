using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
public class TransitionDipCommandTests : SerializedCommandTestBase<TransitionDipCommand,
    TransitionDipCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public byte Rate { get; set; }
        public ushort Input { get; set; }
    }

    protected override TransitionDipCommand CreateSut(TestCaseData testCase)
    {
        // Create state with the required mix effect and transition settings
        var state = new MixEffect
        {
            Index = testCase.Command.Index,
            TransitionSettings =
            {
                Dip =
                {
                    Rate = testCase.Command.Rate,
                    Input = testCase.Command.Input
                }
            }
        };

        // Create command with the mix effect ID
        var command = new TransitionDipCommand(state);

        // Set the actual values that should be written
        command.Rate = testCase.Command.Rate;
        command.Input = testCase.Command.Input;

        return command;
    }

    [Test]
    public void Constructor_WithValidState_InitializesFromState()
    {
        // Arrange
        var state = new MixEffect
        {
            Index = 0,
            TransitionSettings =
            {
                Dip =
                {
                    Rate = 50,
                    Input = 1234
                }
            }
        };

        // Act
        var command = new TransitionDipCommand(state);

        // Assert
        Assert.That(command.MixEffectId, Is.EqualTo(0));
        Assert.That(command.Rate, Is.EqualTo(50));
        Assert.That(command.Input, Is.EqualTo(1234));
        Assert.That(command.Flag, Is.EqualTo(0)); // No flags set when initializing from state
    }


    [Test]
    public void Rate_SetProperty_SetsFlag()
    {
        // Arrange
        var state = new MixEffect
        {
            Index = 0,
            TransitionSettings =
            {
                Dip =
                {
                    Rate = 50,
                    Input = 1234
                }
            }
        };

        var command = new TransitionDipCommand(state);

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
        var state = new MixEffect
        {
            Index = 0,
            TransitionSettings =
            {
                Dip =
                {
                    Rate = 50,
                    Input = 1234
                }
            }
        };
        var command = new TransitionDipCommand(state);

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
        var state = new MixEffect
        {
            Index = 0,
            TransitionSettings =
            {
                Dip =
                {
                    Rate = 50,
                    Input = 1234
                }
            }
        };
        var command = new TransitionDipCommand(state);

        // Act
        command.Rate = 75;
        command.Input = 9999;

        // Assert
        Assert.That(command.Rate, Is.EqualTo(75));
        Assert.That(command.Input, Is.EqualTo(9999));
        Assert.That(command.Flag, Is.EqualTo(3)); // Both flags should be set (1 << 0 | 1 << 1)
    }
}
