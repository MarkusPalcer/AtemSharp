using AtemSharp.Commands.MixEffects;
using AtemSharp.State;

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

    protected override void CompareCommandProperties(PreviewInputUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
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

    [Test]
    public void ApplyToState_WithoutMixEffect_CreatesAndUpdates()
    {
        // Arrange
        const int mixEffectId = 1;
        const int newSource = 2000;

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

        var command = new PreviewInputUpdateCommand
        {
            MixEffectId = mixEffectId,
            Source = newSource
        };

        // Act
        command.ApplyToState(state);

        // Assert
        Assert.That(state.Video, Is.Not.Null);
        Assert.That(state.Video.MixEffects[mixEffectId], Is.Not.Null);
        Assert.That(state.Video.MixEffects[mixEffectId].PreviewInput, Is.EqualTo(newSource));
    }

    [Test]
    public void ApplyToState_WithInvalidMixEffectId_ThrowsInvalidIdError()
    {
        // Arrange
        const int invalidMixEffectId = 10;

        var state = new AtemState
        {
            Info = new DeviceInfo
            {
                Capabilities = new AtemCapabilities
                {
                    MixEffects = 2 // Only 2 mix effects available (0 and 1)
                }
            }
        };

        var command = new PreviewInputUpdateCommand
        {
            MixEffectId = invalidMixEffectId,
            Source = 2000
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidIdError>(() => command.ApplyToState(state));
        Assert.That(exception!.Message, Does.Contain("MixEffect"));
        Assert.That(exception.Message, Does.Contain(invalidMixEffectId.ToString()));
    }

    [Test]
    public void ApplyToState_WithNullCapabilities_ThrowsInvalidIdError()
    {
        // Arrange
        const int mixEffectId = 1;

        var state = new AtemState
        {
            Info = new DeviceInfo
            {
                Capabilities = null // No capabilities
            }
        };

        var command = new PreviewInputUpdateCommand
        {
            MixEffectId = mixEffectId,
            Source = 2000
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidIdError>(() => command.ApplyToState(state));
        Assert.That(exception!.Message, Does.Contain("MixEffect"));
    }

    /// <summary>
    /// Creates an AtemState with a valid mix effect at the specified index
    /// </summary>
    private static AtemState CreateStateWithMixEffect(int mixEffectId, int previewInput = 0)
    {
        Dictionary<int, MixEffect> mixEffects = new Dictionary<int, MixEffect>();
        mixEffects[mixEffectId] = new MixEffect
        {
            Index = mixEffectId,
            ProgramInput = 1000,
            PreviewInput = previewInput,
            TransitionPreview = false,
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

    [Test]
    public void ApplyToState_ValidIndex_ShouldSucceed()
    {
        // Arrange
        var state = new AtemState();
        state.Info.Capabilities = new AtemCapabilities
        {
            MixEffects = 2 // 0-1 valid
        };

        var command = new PreviewInputUpdateCommand
        {
            MixEffectId = 1,
            Source = 2000
        };

        // Act & Assert
        Assert.DoesNotThrow(() => command.ApplyToState(state));
        Assert.That(state.Video.MixEffects[1].PreviewInput, Is.EqualTo(2000));
    }

    [Test]
    public void ApplyToState_InvalidIndex_ShouldThrowInvalidIdError()
    {
        // Arrange
        var state = new AtemState();
        state.Info.Capabilities = new AtemCapabilities
        {
            MixEffects = 2 // 0-1 valid
        };

        var command = new PreviewInputUpdateCommand
        {
            MixEffectId = 5, // Invalid - only 0-1 are valid
            Source = 2000
        };

        // Act & Assert
        var ex = Assert.Throws<InvalidIdError>(() => command.ApplyToState(state));
        Assert.That(ex.Message, Contains.Substring("MixEffect"));
        Assert.That(ex.Message, Contains.Substring("5"));
    }
}
