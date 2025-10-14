using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.State;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
public class TransitionStingerUpdateCommandTests : DeserializedCommandTestBase<TransitionStingerUpdateCommand,
    TransitionStingerUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public int Source { get; set; }
        public bool PreMultipliedKey { get; set; }
        public double Clip { get; set; }
        public double Gain { get; set; }
        public bool Invert { get; set; }
        public int Preroll { get; set; }
        public int ClipDuration { get; set; }
        public int TriggerPoint { get; set; }
        public int MixRate { get; set; }
    }

    protected override void CompareCommandProperties(TransitionStingerUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        var failures = new List<string>();

        // Compare Index to MixEffectId - exact match
        if (!actualCommand.MixEffectId.Equals(expectedData.Index))
        {
            failures.Add($"MixEffectId: expected {expectedData.Index}, actual {actualCommand.MixEffectId}");
        }

        // Compare Source - exact match (integer value)
        if (!actualCommand.Source.Equals(expectedData.Source))
        {
            failures.Add($"Source: expected {expectedData.Source}, actual {actualCommand.Source}");
        }

        // Compare PreMultipliedKey - exact match (boolean value)
        if (!actualCommand.PreMultipliedKey.Equals(expectedData.PreMultipliedKey))
        {
            failures.Add($"PreMultipliedKey: expected {expectedData.PreMultipliedKey}, actual {actualCommand.PreMultipliedKey}");
        }

        // Compare Clip - floating point value so we approximate with 1 decimal place precision
        if (!Utilities.AreApproximatelyEqual(actualCommand.Clip, expectedData.Clip, 1))
        {
            failures.Add($"Clip: expected {expectedData.Clip}, actual {actualCommand.Clip}");
        }

        // Compare Gain - floating point value so we approximate with 1 decimal place precision
        if (!Utilities.AreApproximatelyEqual(actualCommand.Gain, expectedData.Gain, 1))
        {
            failures.Add($"Gain: expected {expectedData.Gain}, actual {actualCommand.Gain}");
        }

        // Compare Invert - exact match (boolean value)
        if (!actualCommand.Invert.Equals(expectedData.Invert))
        {
            failures.Add($"Invert: expected {expectedData.Invert}, actual {actualCommand.Invert}");
        }

        // Compare Preroll - exact match (integer value)
        if (!actualCommand.Preroll.Equals(expectedData.Preroll))
        {
            failures.Add($"Preroll: expected {expectedData.Preroll}, actual {actualCommand.Preroll}");
        }

        // Compare ClipDuration - exact match (integer value)
        if (!actualCommand.ClipDuration.Equals(expectedData.ClipDuration))
        {
            failures.Add($"ClipDuration: expected {expectedData.ClipDuration}, actual {actualCommand.ClipDuration}");
        }

        // Compare TriggerPoint - exact match (integer value)
        if (!actualCommand.TriggerPoint.Equals(expectedData.TriggerPoint))
        {
            failures.Add($"TriggerPoint: expected {expectedData.TriggerPoint}, actual {actualCommand.TriggerPoint}");
        }

        // Compare MixRate - exact match (integer value)
        if (!actualCommand.MixRate.Equals(expectedData.MixRate))
        {
            failures.Add($"MixRate: expected {expectedData.MixRate}, actual {actualCommand.MixRate}");
        }

        // Assert results
        if (failures.Count > 0)
        {
            Assert.Fail($"Command deserialization property mismatch for version {testCase.FirstVersion}:\n" +
                       string.Join("\n", failures));
        }
    }

    [Test]
    public void ApplyToState_ValidState_UpdatesCorrectly()
    {
        // Arrange
        var state = new AtemState
        {
            Video = new VideoState
            {
                MixEffects = new Dictionary<int, MixEffect>
                {
                    [1] = new MixEffect
                    {
                        Index = 1,
                        TransitionSettings = new TransitionSettings()
                    }
                }
            }
        };

        var command = new TransitionStingerUpdateCommand
        {
            MixEffectId = 1,
            Source = 5,
            PreMultipliedKey = true,
            Clip = 25.5,
            Gain = 75.0,
            Invert = true,
            Preroll = 1000,
            ClipDuration = 2000,
            TriggerPoint = 1500,
            MixRate = 30
        };

        // Act
        command.ApplyToState(state);

        // Assert
        var stingerSettings = state.Video.MixEffects[1].TransitionSettings!.Stinger;
        Assert.That(stingerSettings, Is.Not.Null);
        Assert.That(stingerSettings!.Source, Is.EqualTo(5));
        Assert.That(stingerSettings.PreMultipliedKey, Is.EqualTo(true));
        Assert.That(stingerSettings.Clip, Is.EqualTo(25.5));
        Assert.That(stingerSettings.Gain, Is.EqualTo(75.0));
        Assert.That(stingerSettings.Invert, Is.EqualTo(true));
        Assert.That(stingerSettings.Preroll, Is.EqualTo(1000));
        Assert.That(stingerSettings.ClipDuration, Is.EqualTo(2000));
        Assert.That(stingerSettings.TriggerPoint, Is.EqualTo(1500));
        Assert.That(stingerSettings.MixRate, Is.EqualTo(30));
    }

    [Test]
    public void ApplyToState_MixEffectNotFound_ThrowsInvalidIdError()
    {
        // Arrange
        var state = new AtemState
        {
            Video = new VideoState
            {
                MixEffects = new Dictionary<int, MixEffect>()
            }
        };

        var command = new TransitionStingerUpdateCommand
        {
            MixEffectId = 1
        };

        // Act & Assert
        Assert.Throws<InvalidIdError>(() => command.ApplyToState(state));
    }

    [Test]
    public void ApplyToState_NoTransitionSettings_CreatesNew()
    {
        // Arrange
        var state = new AtemState
        {
            Video = new VideoState
            {
                MixEffects = new Dictionary<int, MixEffect>
                {
                    [0] = new MixEffect
                    {
                        Index = 0,
                        TransitionSettings = null
                    }
                }
            }
        };

        var command = new TransitionStingerUpdateCommand
        {
            MixEffectId = 0,
            Source = 3
        };

        // Act
        command.ApplyToState(state);

        // Assert
        Assert.That(state.Video.MixEffects[0].TransitionSettings, Is.Not.Null);
        Assert.That(state.Video.MixEffects[0].TransitionSettings!.Stinger, Is.Not.Null);
        Assert.That(state.Video.MixEffects[0].TransitionSettings.Stinger!.Source, Is.EqualTo(3));
    }
}
