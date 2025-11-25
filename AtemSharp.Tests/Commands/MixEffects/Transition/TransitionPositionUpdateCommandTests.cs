using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.State;
using AtemSharp.State.Info;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
public class TransitionPositionUpdateCommandTests : DeserializedCommandTestBase<TransitionPositionUpdateCommand,
    TransitionPositionUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public bool InTransition { get; set; }
        public byte RemainingFrames { get; set; }
        public double HandlePosition { get; set; }
    }

    protected override void CompareCommandProperties(TransitionPositionUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.MixEffectId, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.InTransition, Is.EqualTo(expectedData.InTransition));
        Assert.That(actualCommand.RemainingFrames, Is.EqualTo(expectedData.RemainingFrames));
        Assert.That(actualCommand.HandlePosition, Is.EqualTo(expectedData.HandlePosition).Within(0.0001));
    }

    [Test]
    public void ApplyToState_ValidMixEffect_UpdatesState()
    {
        // Arrange
        var state = CreateMinimalState();
        var command = new TransitionPositionUpdateCommand
        {
            MixEffectId = 0,
            InTransition = true,
            RemainingFrames = 15,
            HandlePosition = 0.75
        };

        // Act
        command.ApplyToState(state);

        // Assert
        var mixEffect = state.Video.MixEffects[0];
        Assert.That(mixEffect.TransitionPosition.InTransition, Is.True);
        Assert.That(mixEffect.TransitionPosition.RemainingFrames, Is.EqualTo(15));
        Assert.That(mixEffect.TransitionPosition.HandlePosition, Is.EqualTo(0.75));
    }

    private static AtemState CreateMinimalState()
    {
        return new AtemState
        {
            Info = new DeviceInfo
            {
                Capabilities = new AtemCapabilities
                {
                    MixEffects = 1
                }
            },
            Video = new VideoState
            {
                MixEffects =
                [
                    new MixEffect
                    {
                        Id = 0,
                        TransitionPosition = new TransitionPosition()
                    }
                ]
            }
        };
    }
}
