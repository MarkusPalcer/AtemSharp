using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.State;

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

    protected override void CompareCommandProperties(PreviewTransitionUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
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

    [Test]
    public void ApplyToState_WithoutMixEffect_CreatesAndUpdates()
    {
        // Arrange
        const int mixEffectId = 1;
        const bool newPreview = true;

        var state = new AtemState
        {
            Info = new DeviceInfo
            {
                Capabilities = new AtemCapabilities
                {
                    MixEffects = 4 // Allow mix effect 1
                }
            }
        };

        var command = new PreviewTransitionUpdateCommand
        {
            MixEffectId = mixEffectId,
            Preview = newPreview
        };

        // Act
        command.ApplyToState(state);

        // Assert
        Assert.That(state.Video, Is.Not.Null);
        Assert.That(state.Video.MixEffects[mixEffectId], Is.Not.Null);
        Assert.That(state.Video.MixEffects[mixEffectId].TransitionPreview, Is.EqualTo(newPreview));
    }

    [Test]
    public void ApplyToState_WithInvalidMixEffect_ThrowsInvalidIdError()
    {
        // Arrange
        const int invalidMixEffectId = 5;

        var state = new AtemState
        {
            Info = new DeviceInfo
            {
                Capabilities = new AtemCapabilities
                {
                    MixEffects = 2 // Only supports mix effects 0 and 1
                }
            }
        };

        var command = new PreviewTransitionUpdateCommand
        {
            MixEffectId = invalidMixEffectId,
            Preview = true
        };

        // Act & Assert
        var ex = Assert.Throws<InvalidIdError>(() => command.ApplyToState(state));
        Assert.That(ex.Message, Does.Contain("MixEffect"));
        Assert.That(ex.Message, Does.Contain(invalidMixEffectId.ToString()));
    }

    [Test]
    public void ApplyToState_WithNullCapabilities_ThrowsInvalidIdError()
    {
        // Arrange
        const int mixEffectId = 0;

        var state = new AtemState
        {
            Info = new DeviceInfo
            {
                Capabilities = null // No capabilities defined
            }
        };

        var command = new PreviewTransitionUpdateCommand
        {
            MixEffectId = mixEffectId,
            Preview = true
        };

        // Act & Assert
        var ex = Assert.Throws<InvalidIdError>(() => command.ApplyToState(state));
        Assert.That(ex.Message, Does.Contain("MixEffect"));
        Assert.That(ex.Message, Does.Contain(mixEffectId.ToString()));
    }

    /// <summary>
    /// Creates an AtemState with a valid mix effect at the specified index
    /// </summary>
    private static AtemState CreateStateWithMixEffect(int mixEffectId, bool transitionPreview = false)
    {
        Dictionary<int, MixEffect> mixEffects = new Dictionary<int, MixEffect>();
        mixEffects[mixEffectId] = new MixEffect
        {
            Index = mixEffectId,
            ProgramInput = 1000,
            PreviewInput = 2000,
            TransitionPreview = transitionPreview,
            TransitionPosition = new TransitionPosition
            {
                InTransition = false,
                HandlePosition = 0,
                RemainingFrames = 0
            },
        };

        return new AtemState
        {
            Info = new DeviceInfo
            {
                Capabilities = new AtemCapabilities
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
}
