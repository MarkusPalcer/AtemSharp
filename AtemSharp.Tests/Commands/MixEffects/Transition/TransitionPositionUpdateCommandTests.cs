using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.State;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
public class TransitionPositionUpdateCommandTests : DeserializedCommandTestBase<TransitionPositionUpdateCommand,
    TransitionPositionUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public bool InTransition { get; set; }
        public int RemainingFrames { get; set; }
        public double HandlePosition { get; set; }
    }

    protected override void CompareCommandProperties(TransitionPositionUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        var failures = new List<string>();

        // Compare Index to MixEffectId - exact match
        if (!actualCommand.MixEffectId.Equals(expectedData.Index))
        {
            failures.Add($"MixEffectId: expected {expectedData.Index}, actual {actualCommand.MixEffectId}");
        }

        // Compare InTransition - exact match
        if (!actualCommand.InTransition.Equals(expectedData.InTransition))
        {
            failures.Add($"InTransition: expected {expectedData.InTransition}, actual {actualCommand.InTransition}");
        }

        // Compare RemainingFrames - exact match
        if (!actualCommand.RemainingFrames.Equals(expectedData.RemainingFrames))
        {
            failures.Add($"RemainingFrames: expected {expectedData.RemainingFrames}, actual {actualCommand.RemainingFrames}");
        }

        // Compare HandlePosition - floating point value so we approximate
        if (!Utilities.AreApproximatelyEqual(actualCommand.HandlePosition, expectedData.HandlePosition))
        {
            failures.Add($"HandlePosition: expected {expectedData.HandlePosition}, actual {actualCommand.HandlePosition}");
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

    [Test]
    public void ApplyToState_InvalidMixEffect_ThrowsException()
    {
        // Arrange
        var state = CreateMinimalState();
        var command = new TransitionPositionUpdateCommand
        {
            MixEffectId = 1, // Only MixEffect 0 exists
            InTransition = true,
            RemainingFrames = 15,
            HandlePosition = 0.75
        };

        // Act & Assert
        var ex = Assert.Throws<InvalidIdError>(() => command.ApplyToState(state));
        Assert.That(ex!.Message, Contains.Substring("MixEffect"));
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
                MixEffects = new Dictionary<int, MixEffect>
                {
                    [0] = new MixEffect
                    {
                        Index = 0,
                        TransitionPosition = new TransitionPosition()
                    }
                }
            }
        };
    }
}
