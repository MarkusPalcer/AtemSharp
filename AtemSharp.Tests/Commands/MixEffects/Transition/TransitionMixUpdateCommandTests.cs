using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
public class TransitionMixUpdateCommandTests : DeserializedCommandTestBase<TransitionMixUpdateCommand,
    TransitionMixUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public int Rate { get; set; }
    }

    protected override void CompareCommandProperties(TransitionMixUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.MixEffectId, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.Rate, Is.EqualTo(expectedData.Rate));
    }

    [Test]
    public void ApplyToState_ValidMixEffect_UpdatesState()
    {
        // Arrange
        const int mixEffectId = 1;
        const int newRate = 100;

        var state = CreateValidAtemState(mixEffectId);
        var command = new TransitionMixUpdateCommand
        {
            MixEffectId = mixEffectId,
            Rate = newRate
        };

        // Act
        command.ApplyToState(state);

        // Assert - check the state was updated
        Assert.That(state.Video.MixEffects[mixEffectId].TransitionSettings, Is.Not.Null);
        Assert.That(state.Video.MixEffects[mixEffectId].TransitionSettings.Mix, Is.Not.Null);
        Assert.That(state.Video.MixEffects[mixEffectId].TransitionSettings.Mix.Rate, Is.EqualTo(newRate));
    }

    /// <summary>
    /// Creates a valid AtemState with a mix effect for testing
    /// </summary>
    private static AtemState CreateValidAtemState(byte mixEffectId)
    {
        var mixEffects = new MixEffect[mixEffectId + 1];
        mixEffects[mixEffectId] = new MixEffect
        {
            Id = mixEffectId,
            ProgramInput = 1000,
            PreviewInput = 1001,
            TransitionPreview = false,
            TransitionPosition = new TransitionPosition
            {
                InTransition = false,
                RemainingFrames = 0,
                HandlePosition = 0.0
            },
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
}
