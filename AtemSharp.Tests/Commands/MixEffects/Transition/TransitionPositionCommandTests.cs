using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.State.Video.MixEffect;

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

    protected override TransitionPositionCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new TransitionPositionCommand(new MixEffect
        {
            Id = testCase.Command.Index,
            TransitionPosition =
            {
                HandlePosition = testCase.Command.HandlePosition
            }
        });
    }

    [Test]
    public void HandlePosition_Setter_SetsFlag()
    {
        // Arrange
        var state = new MixEffect
        {
            Id = 0,
            TransitionPosition =
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
