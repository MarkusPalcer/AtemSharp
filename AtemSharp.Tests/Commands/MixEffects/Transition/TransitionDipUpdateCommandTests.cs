using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
public class TransitionDipUpdateCommandTests : DeserializedCommandTestBase<TransitionDipUpdateCommand,
    TransitionDipUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public int Rate { get; set; }
        public int Input { get; set; }
    }

    protected override void CompareCommandProperties(TransitionDipUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
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

        // Compare Input - exact match (integer value)
        if (!actualCommand.Input.Equals(expectedData.Input))
        {
            failures.Add($"Input: expected {expectedData.Input}, actual {actualCommand.Input}");
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
        using var stream = new MemoryStream(new byte[] {
            0x01,       // MixEffectId = 1
            0x32,       // Rate = 50
            0x04,       // Input high byte (0x0444 = 1092)
            0x44        // Input low byte
        });

        // Act
        var command = TransitionDipUpdateCommand.Deserialize(stream, ProtocolVersion.V8_1_1);

        // Assert
        Assert.That(command.MixEffectId, Is.EqualTo(1));
        Assert.That(command.Rate, Is.EqualTo(50));
        Assert.That(command.Input, Is.EqualTo(1092)); // 0x0444
    }

    [Test]
    public void ApplyToState_ValidState_UpdatesDipSettings()
    {
        // Arrange
        var command = new TransitionDipUpdateCommand
        {
            MixEffectId = 0,
            Rate = 75,
            Input = 2048
        };

        var state = new AtemState
        {
            Video = new VideoState
            {
                MixEffects = new Dictionary<int, MixEffect>
                {
                    [0] = new MixEffect
                    {
                        Index = 0,
                        TransitionSettings = new TransitionSettings()
                    }
                }
            }
        };

        // Act
        command.ApplyToState(state);

        // Assert
        Assert.That(state.Video.MixEffects[0].TransitionSettings.Dip, Is.Not.Null);
        Assert.That(state.Video.MixEffects[0].TransitionSettings.Dip.Rate, Is.EqualTo(75));
        Assert.That(state.Video.MixEffects[0].TransitionSettings.Dip.Input, Is.EqualTo(2048));
    }

    [Test]
    public void ApplyToState_MissingTransitionSettings_CreatesNewSettings()
    {
        // Arrange
        var command = new TransitionDipUpdateCommand
        {
            MixEffectId = 1,
            Rate = 100,
            Input = 3000
        };

        var state = new AtemState
        {
            Video = new VideoState
            {
                MixEffects = new Dictionary<int, MixEffect>
                {
                    [1] = new MixEffect
                    {
                        Index = 1,
                        TransitionSettings = null // Missing settings
                    }
                }
            }
        };

        // Act
        command.ApplyToState(state);

        // Assert
        Assert.That(state.Video.MixEffects[1].TransitionSettings, Is.Not.Null);
        Assert.That(state.Video.MixEffects[1].TransitionSettings!.Dip, Is.Not.Null);
        Assert.That(state.Video.MixEffects[1].TransitionSettings!.Dip!.Rate, Is.EqualTo(100));
        Assert.That(state.Video.MixEffects[1].TransitionSettings!.Dip!.Input, Is.EqualTo(3000));
    }

    [Test]
    public void ApplyToState_InvalidMixEffectId_ThrowsInvalidIdError()
    {
        // Arrange
        var command = new TransitionDipUpdateCommand
        {
            MixEffectId = 99, // Invalid mix effect ID
            Rate = 50,
            Input = 1000
        };

        var state = new AtemState
        {
            Video = new VideoState
            {
                MixEffects = new Dictionary<int, MixEffect>() // Empty mix effects
            }
        };

        // Act & Assert
        var ex = Assert.Throws<InvalidIdError>(() => command.ApplyToState(state));
        Assert.That(ex.Message, Does.Contain("MixEffect"));
        Assert.That(ex.Message, Does.Contain("99"));
    }

    [Test]
    public void Deserialize_EdgeCaseValues_HandlesCorrectly()
    {
        // Arrange - test with edge case values
        using var stream = new MemoryStream(new byte[] {
            0xFF,       // MixEffectId = 255 (max byte value)
            0x00,       // Rate = 0 (minimum)
            0xFF,       // Input high byte (0xFFFF = 65535)
            0xFF        // Input low byte
        });

        // Act
        var command = TransitionDipUpdateCommand.Deserialize(stream, ProtocolVersion.V8_1_1);

        // Assert
        Assert.That(command.MixEffectId, Is.EqualTo(255));
        Assert.That(command.Rate, Is.EqualTo(0));
        Assert.That(command.Input, Is.EqualTo(65535)); // 0xFFFF
    }
}
