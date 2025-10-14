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

    protected override void CompareCommandProperties(TransitionMixUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        var failures = new List<string>();

        // Compare Index to MixEffectId - exact match
        if (!actualCommand.MixEffectId.Equals(expectedData.Index))
        {
            failures.Add($"MixEffectId: expected {expectedData.Index}, actual {actualCommand.MixEffectId}");
        }

        // Compare Rate - exact match (integer value)
        if (!actualCommand.Rate.Equals(expectedData.Rate))
        {
            failures.Add($"Rate: expected {expectedData.Rate}, actual {actualCommand.Rate}");
        }

        // Assert results
        if (failures.Count > 0)
        {
            Assert.Fail($"Command deserialization property mismatch for version {testCase.FirstVersion}:\n" +
                       string.Join("\n", failures));
        }
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
        Assert.That(state.Video.MixEffects[mixEffectId].TransitionSettings!.Mix, Is.Not.Null);
        Assert.That(state.Video.MixEffects[mixEffectId].TransitionSettings!.Mix!.Rate, Is.EqualTo(newRate));
    }

    [Test]
    public void ApplyToState_MissingTransitionSettings_CreatesSettings()
    {
        // Arrange
        const int mixEffectId = 0;
        const int newRate = 75;

        var state = CreateValidAtemState(mixEffectId);
        state.Video.MixEffects[mixEffectId].TransitionSettings = null; // Remove transition settings

        var command = new TransitionMixUpdateCommand
        {
            MixEffectId = mixEffectId,
            Rate = newRate
        };

        // Act
        command.ApplyToState(state);

        // Assert - check the settings were created and updated
        Assert.That(state.Video.MixEffects[mixEffectId].TransitionSettings, Is.Not.Null);
        Assert.That(state.Video.MixEffects[mixEffectId].TransitionSettings!.Mix, Is.Not.Null);
        Assert.That(state.Video.MixEffects[mixEffectId].TransitionSettings!.Mix!.Rate, Is.EqualTo(newRate));
    }

    [Test]
    public void ApplyToState_InvalidMixEffect_ThrowsException()
    {
        // Arrange
        const int mixEffectId = 99; // Invalid mix effect ID
        var state = CreateValidAtemState(0);
        var command = new TransitionMixUpdateCommand
        {
            MixEffectId = mixEffectId,
            Rate = 50
        };

        // Act & Assert
        var ex = Assert.Throws<InvalidIdError>(() => command.ApplyToState(state));
        Assert.That(ex.Message, Does.Contain("MixEffect"));
        Assert.That(ex.Message, Does.Contain(mixEffectId.ToString()));
    }

    /// <summary>
    /// Creates a valid AtemState with a mix effect for testing
    /// </summary>
    private static AtemState CreateValidAtemState(int mixEffectId)
    {
        Dictionary<int, MixEffect> mixEffects = new Dictionary<int, MixEffect>();
        mixEffects[mixEffectId] = new MixEffect
        {
            Index = mixEffectId,
            ProgramInput = 1000,
            PreviewInput = 1001,
            TransitionPreview = false,
            TransitionPosition = new TransitionPosition
            {
                InTransition = false,
                RemainingFrames = 0,
                HandlePosition = 0.0
            },
            TransitionSettings = new TransitionSettings()
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
