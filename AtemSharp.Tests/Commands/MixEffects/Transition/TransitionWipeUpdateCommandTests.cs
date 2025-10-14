using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.Enums;
using AtemSharp.State;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
public class TransitionWipeUpdateCommandTests : DeserializedCommandTestBase<TransitionWipeUpdateCommand,
    TransitionWipeUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public int Rate { get; set; }
        public int Pattern { get; set; }
        public double BorderWidth { get; set; }
        public int BorderInput { get; set; }
        public double Symmetry { get; set; }
        public double BorderSoftness { get; set; }
        public double XPosition { get; set; }
        public double YPosition { get; set; }
        public bool ReverseDirection { get; set; }
        public bool FlipFlop { get; set; }
    }

    protected override void CompareCommandProperties(TransitionWipeUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
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

        // Compare Pattern - exact match (integer value)
        if (!actualCommand.Pattern.Equals(expectedData.Pattern))
        {
            failures.Add($"Pattern: expected {expectedData.Pattern}, actual {actualCommand.Pattern}");
        }

        // Compare BorderWidth - floating point value so we approximate
        if (!Utilities.AreApproximatelyEqual(actualCommand.BorderWidth, expectedData.BorderWidth))
        {
            failures.Add($"BorderWidth: expected {expectedData.BorderWidth}, actual {actualCommand.BorderWidth}");
        }

        // Compare BorderInput - exact match (integer value)
        if (!actualCommand.BorderInput.Equals(expectedData.BorderInput))
        {
            failures.Add($"BorderInput: expected {expectedData.BorderInput}, actual {actualCommand.BorderInput}");
        }

        // Compare Symmetry - floating point value so we approximate
        if (!Utilities.AreApproximatelyEqual(actualCommand.Symmetry, expectedData.Symmetry))
        {
            failures.Add($"Symmetry: expected {expectedData.Symmetry}, actual {actualCommand.Symmetry}");
        }

        // Compare BorderSoftness - floating point value so we approximate
        if (!Utilities.AreApproximatelyEqual(actualCommand.BorderSoftness, expectedData.BorderSoftness))
        {
            failures.Add($"BorderSoftness: expected {expectedData.BorderSoftness}, actual {actualCommand.BorderSoftness}");
        }

        // Compare XPosition - floating point value so we approximate
        if (!Utilities.AreApproximatelyEqual(actualCommand.XPosition, expectedData.XPosition))
        {
            failures.Add($"XPosition: expected {expectedData.XPosition}, actual {actualCommand.XPosition}");
        }

        // Compare YPosition - floating point value so we approximate
        if (!Utilities.AreApproximatelyEqual(actualCommand.YPosition, expectedData.YPosition))
        {
            failures.Add($"YPosition: expected {expectedData.YPosition}, actual {actualCommand.YPosition}");
        }

        // Compare ReverseDirection - exact match (boolean value)
        if (!actualCommand.ReverseDirection.Equals(expectedData.ReverseDirection))
        {
            failures.Add($"ReverseDirection: expected {expectedData.ReverseDirection}, actual {actualCommand.ReverseDirection}");
        }

        // Compare FlipFlop - exact match (boolean value)
        if (!actualCommand.FlipFlop.Equals(expectedData.FlipFlop))
        {
            failures.Add($"FlipFlop: expected {expectedData.FlipFlop}, actual {actualCommand.FlipFlop}");
        }

        // Assert results
        if (failures.Count > 0)
        {
            Assert.Fail($"Command deserialization property mismatch for version {testCase.FirstVersion}:\n" +
                       string.Join("\n", failures));
        }
    }

    [Test]
    public void Deserialize_ValidData_ProducesCorrectCommand()
    {
        // Arrange - create binary data matching the ATEM protocol
        // Based on test data: "00-A1-0A-00-26-25-00-00-25-46-17-71-26-C4-03-67-00-01"
        using var stream = new MemoryStream(new byte[] {
            0x00,       // MixEffectId = 0
            0xA1,       // Rate = 161
            0x0A,       // Pattern = 10
            0x00,       // Padding
            0x26, 0x25, // BorderWidth = 9765 (97.65%)
            0x00, 0x00, // BorderInput = 0
            0x25, 0x46, // Symmetry = 9542 (95.42%)
            0x17, 0x71, // BorderSoftness = 6001 (60.01%)
            0x26, 0xC4, // XPosition = 9924 (0.9924)
            0x03, 0x67, // YPosition = 871 (0.0871)
            0x00,       // ReverseDirection = false
            0x01        // FlipFlop = true
        });

        // Act
        var command = TransitionWipeUpdateCommand.Deserialize(stream, ProtocolVersion.V8_1_1);

        // Assert
        Assert.That(command.MixEffectId, Is.EqualTo(0));
        Assert.That(command.Rate, Is.EqualTo(161));
        Assert.That(command.Pattern, Is.EqualTo(10));
        Assert.That(command.BorderWidth, Is.EqualTo(97.65).Within(0.01));
        Assert.That(command.BorderInput, Is.EqualTo(0));
        Assert.That(command.Symmetry, Is.EqualTo(95.42).Within(0.01));
        Assert.That(command.BorderSoftness, Is.EqualTo(60.01).Within(0.01));
        Assert.That(command.XPosition, Is.EqualTo(0.9924).Within(0.0001));
        Assert.That(command.YPosition, Is.EqualTo(0.0871).Within(0.0001));
        Assert.That(command.ReverseDirection, Is.False);
        Assert.That(command.FlipFlop, Is.True);
    }

    [Test]
    public void ApplyToState_ValidState_UpdatesWipeSettings()
    {
        // Arrange
        var command = new TransitionWipeUpdateCommand
        {
            MixEffectId = 0,
            Rate = 75,
            Pattern = 5,
            BorderWidth = 12.5,
            BorderInput = 2048,
            Symmetry = 65.3,
            BorderSoftness = 33.7,
            XPosition = 0.7856,
            YPosition = 0.2341,
            ReverseDirection = true,
            FlipFlop = false
        };

        var state = new AtemState
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
                        ProgramInput = 1000,
                        PreviewInput = 1001
                    }
                }
            }
        };

        // Act
        command.ApplyToState(state);

        // Assert
        var mixEffect = state.Video.MixEffects[0];
        Assert.That(mixEffect.TransitionSettings, Is.Not.Null);
        Assert.That(mixEffect.TransitionSettings.Wipe, Is.Not.Null);

        var wipeSettings = mixEffect.TransitionSettings.Wipe;
        Assert.That(wipeSettings.Rate, Is.EqualTo(75));
        Assert.That(wipeSettings.Pattern, Is.EqualTo(5));
        Assert.That(wipeSettings.BorderWidth, Is.EqualTo(12.5));
        Assert.That(wipeSettings.BorderInput, Is.EqualTo(2048));
        Assert.That(wipeSettings.Symmetry, Is.EqualTo(65.3));
        Assert.That(wipeSettings.BorderSoftness, Is.EqualTo(33.7));
        Assert.That(wipeSettings.XPosition, Is.EqualTo(0.7856));
        Assert.That(wipeSettings.YPosition, Is.EqualTo(0.2341));
        Assert.That(wipeSettings.ReverseDirection, Is.True);
        Assert.That(wipeSettings.FlipFlop, Is.False);
    }

    [Test]
    public void ApplyToState_InvalidMixEffect_ThrowsException()
    {
        // Arrange
        var command = new TransitionWipeUpdateCommand
        {
            MixEffectId = 5, // Invalid mix effect index
            Rate = 50
        };

        var state = new AtemState
        {
            Info = new DeviceInfo
            {
                Capabilities = new AtemCapabilities
                {
                    MixEffects = 2  // Only 2 mix effects available (0, 1)
                }
            },
            Video = new VideoState
            {
                MixEffects = new Dictionary<int, MixEffect>()
            }
        };

        // Act & Assert
        Assert.Throws<InvalidIdError>(() => command.ApplyToState(state));
    }

    [Test]
    public void ApplyToState_InitializesTransitionSettingsWhenMissing()
    {
        // Arrange
        var command = new TransitionWipeUpdateCommand
        {
            MixEffectId = 0,
            Rate = 100
        };

        var state = new AtemState
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
                        TransitionSettings = null  // No transition settings
                    }
                }
            }
        };

        // Act
        command.ApplyToState(state);

        // Assert
        var mixEffect = state.Video.MixEffects[0];
        Assert.That(mixEffect.TransitionSettings, Is.Not.Null, "TransitionSettings should be initialized");
        Assert.That(mixEffect.TransitionSettings.Wipe, Is.Not.Null, "Wipe settings should be initialized");
        Assert.That(mixEffect.TransitionSettings.Wipe.Rate, Is.EqualTo(100));
    }
}
