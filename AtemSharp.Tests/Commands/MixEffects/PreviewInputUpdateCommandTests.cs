using AtemSharp.Commands.MixEffects;
using AtemSharp.State;
using AtemSharp.State.Video;
using AtemSharp.State.Video.MixEffect;

namespace AtemSharp.Tests.Commands.MixEffects;

[TestFixture]
public class PreviewInputUpdateCommandTests : DeserializedCommandTestBase<PreviewInputUpdateCommand,
    PreviewInputUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int Index { get; set; }
        public int Source { get; set; }
    }

    protected override void CompareCommandProperties(PreviewInputUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.MixEffectId, Is.EqualTo(expectedData.Index),
                    $"MixEffectId should match expected value for test case {testCase.Name}");
        Assert.That(actualCommand.Source, Is.EqualTo(expectedData.Source),
                    $"Source should match expected value for test case {testCase.Name}");
    }

    [Test]
    public void ApplyToState_WithValidMixEffect_UpdatesPreviewInput()
    {
        // Arrange
        const int mixEffectId = 1;
        const int newSource = 2000;

        var state = CreateStateWithMixEffect(mixEffectId, 1000); // Initial source
        var command = new PreviewInputUpdateCommand
        {
            MixEffectId = mixEffectId,
            Source = newSource
        };

        // Act
        command.ApplyToState(state);

        // Assert
        Assert.That(state.Video.MixEffects[mixEffectId].PreviewInput, Is.EqualTo(newSource));
    }

    /// <summary>
    /// Creates an AtemState with a valid mix effect at the specified index
    /// </summary>
    private static AtemState CreateStateWithMixEffect(byte mixEffectId, ushort previewInput = 0)
    {
        var mixEffects = new MixEffect[mixEffectId + 1];
        mixEffects[mixEffectId] = new MixEffect
        {
            Id = mixEffectId,
            ProgramInput = 1000,
            PreviewInput = previewInput,
            TransitionPreview = false,
            TransitionPosition =
            {
                InTransition = false,
                HandlePosition = 0,
                RemainingFrames = 0
            },
        };

        return new AtemState
        {
            Info =
            {
                Capabilities =
                {
                    MixEffects = Math.Max(mixEffectId + 1, 2)
                }
            },
            Video = new VideoState
            {
                MixEffects = mixEffects
            }
        };
    }

    [Test]
    public void ApplyToState_ValidIndex_ShouldSucceed()
    {
        // Arrange
        var state = new AtemState
        {
            Video =
            {
                MixEffects = AtemStateUtil.CreateArray<MixEffect>(2)
            }
        };

        var command = new PreviewInputUpdateCommand
        {
            MixEffectId = 1,
            Source = 2000
        };

        // Act & Assert
        command.ApplyToState(state);
        Assert.That(state.Video.MixEffects[1].PreviewInput, Is.EqualTo(2000));
    }
}
