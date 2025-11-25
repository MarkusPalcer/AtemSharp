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
        public byte Index { get; set; }
        public TransitionStyle NextStyle { get; set; }
        public TransitionSelection NextSelection { get; set; }
    }

    protected override TransitionPropertiesCommand CreateSut(TestCaseData testCase)
    {
        return new TransitionPropertiesCommand(new MixEffect
        {
            Id = testCase.Command.Index,
            TransitionProperties =
            {
                NextStyle = testCase.Command.NextStyle,
                NextSelection = testCase.Command.NextSelection
            }
        });
    }

    [Test]
    public void Constructor_WithValidMixEffect_InitializesFromState()
    {
        // Arrange
        const int mixEffectId = 1;
        const TransitionStyle expectedNextStyle = TransitionStyle.Wipe;
        const TransitionSelection expectedNextSelection = TransitionSelection.Key1 | TransitionSelection.Background;
        var state = new MixEffect
        {
            Id = mixEffectId,
            TransitionProperties =
            {
                NextStyle = expectedNextStyle,
                NextSelection = expectedNextSelection,
            }
        };

        // Act
        var command = new TransitionPropertiesCommand(state);

        // Assert
        Assert.That(command.MixEffectId, Is.EqualTo(mixEffectId));
        Assert.That(command.NextStyle, Is.EqualTo(expectedNextStyle));
        Assert.That(command.NextSelection, Is.EqualTo(expectedNextSelection));
        Assert.That(command.Flag, Is.EqualTo(0), "Flag should not be set when initializing from state");
    }

    [Test]
    public void NextStyle_WhenSet_UpdatesFlagAutomatically()
    {
        // Arrange
        var state = new MixEffect
        {
            Id = 0,
            TransitionProperties =
            {
                NextStyle = TransitionStyle.DVE,
                NextSelection = TransitionSelection.Background
            }
        };

        var command = new TransitionPropertiesCommand(state);
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
        var state = new MixEffect
        {
            Id = 0,
            TransitionProperties =
            {
                NextStyle = TransitionStyle.DVE,
                NextSelection = TransitionSelection.Background
            }
        };

        var command = new TransitionPropertiesCommand(state);
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
        var state = new MixEffect
        {
            Id = 0,
            TransitionProperties =
            {
                NextStyle = TransitionStyle.DVE,
                NextSelection = TransitionSelection.Background
            }
        };

        var command = new TransitionPropertiesCommand(state);
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
