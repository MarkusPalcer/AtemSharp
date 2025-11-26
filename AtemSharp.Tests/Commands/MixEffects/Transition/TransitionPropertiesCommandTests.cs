using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.State.Video.MixEffect;
using AtemSharp.State.Video.MixEffect.Transition;

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
    public void NextStyle_WhenSet_UpdatesFlagAutomatically()
    {
        // Arrange
        var command = new TransitionPropertiesCommand(new MixEffect());

        // Act
        command.NextStyle = TransitionStyle.Dip;

        // Assert
        Assert.That(command.Flag, Is.EqualTo(1), "Flag should have NextStyle bit set");
    }

    [Test]
    public void NextSelection_WhenSet_UpdatesFlagAutomatically()
    {
        // Arrange
        var command = new TransitionPropertiesCommand(new MixEffect());

        // Act
        command.NextSelection = TransitionSelection.Key1 | TransitionSelection.Key2;

        // Assert
        Assert.That(command.Flag, Is.EqualTo(2), "Flag should have NextSelection bit set");
    }

    [Test]
    public void BothProperties_WhenSet_UpdatesFlagsCombined()
    {
        // Arrange
        var command = new TransitionPropertiesCommand(new MixEffect());

        // Act
        command.NextStyle = TransitionStyle.DVE;
        command.NextSelection = TransitionSelection.Background | TransitionSelection.Key3;

        // Assert
        Assert.That(command.Flag, Is.EqualTo(3), "Flag should have both bits set");
    }
}
