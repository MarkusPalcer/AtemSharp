using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.State.Video.MixEffect;

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
        return new TransitionMixCommand(new MixEffect
        {
            Id = testCase.Command.Index,
            TransitionSettings =
            {
                Mix =
                {
                    Rate = testCase.Command.Rate
                }
            }
        });
    }

    [Test]
    public void Rate_SetProperty_SetsFlag()
    {
        // Arrange
        var command = new TransitionMixCommand(new MixEffect());

        // Act
        command.Rate = 100;

        // Assert
        Assert.That(command.Flag, Is.EqualTo(1)); // Flag should be set
    }
}
