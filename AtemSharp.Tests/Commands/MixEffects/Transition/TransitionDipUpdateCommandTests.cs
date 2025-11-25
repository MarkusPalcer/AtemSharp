using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
public class TransitionDipUpdateCommandTests : DeserializedCommandTestBase<TransitionDipUpdateCommand,
    TransitionDipUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public byte Rate { get; set; }
        public ushort Input { get; set; }
    }

    protected override void CompareCommandProperties(TransitionDipUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.MixEffectId, Is.EqualTo(testCase.Command.Index));
        Assert.That(actualCommand.Rate, Is.EqualTo(testCase.Command.Rate));
        Assert.That(actualCommand.Input, Is.EqualTo(testCase.Command.Input));
    }

    [Test]
    public void ApplyToState_ValidState_UpdatesDipSettings()
    {
        // Arrange
        var command = new TransitionDipUpdateCommand
        {
            MixEffectId = 0,
            Rate = 75,
            Input = 2048
        };

        var state = new AtemState
        {
            Video = new VideoState
            {
                MixEffects =
                [
                    new MixEffect
                    {
                        Id = 0,
                    }
                ]
            }
        };

        // Act
        command.ApplyToState(state);

        // Assert
        Assert.That(state.Video.MixEffects[0].TransitionSettings.Dip, Is.Not.Null);
        Assert.That(state.Video.MixEffects[0].TransitionSettings.Dip.Rate, Is.EqualTo(75));
        Assert.That(state.Video.MixEffects[0].TransitionSettings.Dip.Input, Is.EqualTo(2048));
    }

    [Test]
    public void ApplyToState_InvalidMixEffectId_ThrowsInvalidIdError()
    {
        // Arrange
        var command = new TransitionDipUpdateCommand
        {
            MixEffectId = 99, // Invalid mix effect ID
            Rate = 50,
            Input = 1000
        };

        var state = new AtemState
        {
            Video = new VideoState
            {
                MixEffects = [] // Empty mix effects
            }
        };

        // Act & Assert
        Assert.Throws<IndexOutOfRangeException>(() => command.ApplyToState(state));
    }
}
