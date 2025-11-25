using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.State;
using AtemSharp.State.Video;
using AtemSharp.State.Video.MixEffect;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
public class PreviewTransitionUpdateCommandTests : DeserializedCommandTestBase<PreviewTransitionUpdateCommand,
    PreviewTransitionUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int Index { get; set; }
        public bool PreviewTransition { get; set; }
    }

    protected override void CompareCommandProperties(PreviewTransitionUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.MixEffectId, Is.EqualTo(expectedData.Index),
                    $"MixEffectId should match expected value for test case {testCase.Name}");
        Assert.That(actualCommand.Preview, Is.EqualTo(expectedData.PreviewTransition),
                    $"Preview should match expected value for test case {testCase.Name}");
    }

    [Test]
    public void ApplyToState_WithValidMixEffect_UpdatesTransitionPreview()
    {
        // Arrange
        const int mixEffectId = 1;
        const bool newPreview = true;

        var state = CreateStateWithMixEffect(mixEffectId); // Initial state: preview disabled
        var command = new PreviewTransitionUpdateCommand
        {
            MixEffectId = mixEffectId,
            Preview = newPreview
        };

        // Act
        command.ApplyToState(state);

        // Assert
        Assert.That(state.Video.MixEffects[mixEffectId].TransitionPreview, Is.EqualTo(newPreview));
    }

    /// <summary>
    /// Creates an AtemState with a valid mix effect at the specified index
    /// </summary>
    private static AtemState CreateStateWithMixEffect(byte mixEffectId, bool transitionPreview = false)
    {
        var mixEffects = new MixEffect[mixEffectId + 1];
        mixEffects[mixEffectId] = new MixEffect
        {
            Id = mixEffectId,
            ProgramInput = 1000,
            PreviewInput = 2000,
            TransitionPreview = transitionPreview,
            TransitionPosition =
            {
                InTransition = false,
                HandlePosition = 0,
                RemainingFrames = 0
            },
        };

        return new AtemState
        {
            Video = new VideoState
            {
                MixEffects = mixEffects
            }
        };
    }
}
