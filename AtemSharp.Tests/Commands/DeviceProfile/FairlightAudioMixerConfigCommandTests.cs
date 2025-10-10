using AtemSharp.Commands.DeviceProfile;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.DeviceProfile;

[TestFixture]
public class FairlightAudioMixerConfigCommandTests : DeserializedCommandTestBase<FairlightAudioMixerConfigCommand, FairlightAudioMixerConfigCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Inputs { get; set; }
        public byte Monitors { get; set; }
    }

    protected override void CompareCommandProperties(FairlightAudioMixerConfigCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        var failures = new List<string>();

        // Compare Inputs
        if (actualCommand.Inputs != expectedData.Inputs)
        {
            failures.Add($"Inputs: expected {expectedData.Inputs}, actual {actualCommand.Inputs}");
        }

        // Compare Monitors
        if (actualCommand.Monitors != expectedData.Monitors)
        {
            failures.Add($"Monitors: expected {expectedData.Monitors}, actual {actualCommand.Monitors}");
        }

        if (failures.Count > 0)
        {
            Assert.Fail($"Command property comparison failed:\n{string.Join("\n", failures)}");
        }
    }

    [Test]
    public void ApplyToState_WithValidData_ShouldUpdateInfoAndFairlightState()
    {
        // Arrange
        var state = new AtemState();
        var command = new FairlightAudioMixerConfigCommand
        {
            Inputs = 24,
            Monitors = 4
        };

        // Act
        var result = command.ApplyToState(state);

        // Assert
        Assert.That(state.Info.FairlightMixer, Is.Not.Null);
        Assert.That(state.Info.FairlightMixer.Inputs, Is.EqualTo(24));
        Assert.That(state.Info.FairlightMixer.Monitors, Is.EqualTo(4));
        
        Assert.That(state.Fairlight, Is.Not.Null);
        Assert.That(state.Fairlight.Inputs, Is.Not.Null);
        Assert.That(state.Fairlight.Inputs, Is.Empty);
        
        Assert.That(result, Is.EqualTo(new[] { "info.fairlightMixer", "fairlight.inputs" }));
    }

    [Test]
    public void Deserialize_WithTypicalValues_ShouldDeserializeCorrectly()
    {
        // Arrange
        var data = new byte[] { 0x18, 0x04 }; // Inputs: 24, Monitors: 4
        using var stream = new MemoryStream(data);

        // Act
        var command = FairlightAudioMixerConfigCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert
        Assert.That(command, Is.Not.Null);
        Assert.That(command.Inputs, Is.EqualTo(24));
        Assert.That(command.Monitors, Is.EqualTo(4));
    }

    [Test]
    public void Deserialize_WithMinimalValues_ShouldDeserializeCorrectly()
    {
        // Arrange
        var data = new byte[] { 0x00, 0x00 }; // Inputs: 0, Monitors: 0
        using var stream = new MemoryStream(data);

        // Act
        var command = FairlightAudioMixerConfigCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert
        Assert.That(command, Is.Not.Null);
        Assert.That(command.Inputs, Is.EqualTo(0));
        Assert.That(command.Monitors, Is.EqualTo(0));
    }

    [Test]
    public void Deserialize_WithMaxValues_ShouldDeserializeCorrectly()
    {
        // Arrange
        var data = new byte[] { 0xFF, 0xFF }; // Inputs: 255, Monitors: 255
        using var stream = new MemoryStream(data);

        // Act
        var command = FairlightAudioMixerConfigCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert
        Assert.That(command, Is.Not.Null);
        Assert.That(command.Inputs, Is.EqualTo(255));
        Assert.That(command.Monitors, Is.EqualTo(255));
    }

    [Test]
    public void ApplyToState_ShouldNotAffectExistingClassicAudioState()
    {
        // Arrange
        var state = new AtemState
        {
            Audio = new AudioState
            {
                Channels = new Dictionary<int, ClassicAudioChannel>
                {
                    { 1, new ClassicAudioChannel() }
                }
            }
        };

        var command = new FairlightAudioMixerConfigCommand
        {
            Inputs = 24,
            Monitors = 4
        };

        // Act
        command.ApplyToState(state);

        // Assert - Classic audio state should remain unchanged
        Assert.That(state.Audio, Is.Not.Null);
        Assert.That(state.Audio.Channels, Contains.Key(1));
        
        // Fairlight state should be initialized
        Assert.That(state.Fairlight, Is.Not.Null);
        Assert.That(state.Info.FairlightMixer, Is.Not.Null);
    }

    [Test]
    public void ApplyToState_MultipleCallsShouldOverwritePreviousState()
    {
        // Arrange
        var state = new AtemState();
        var command1 = new FairlightAudioMixerConfigCommand { Inputs = 12, Monitors = 2 };
        var command2 = new FairlightAudioMixerConfigCommand { Inputs = 24, Monitors = 4 };

        // Act
        command1.ApplyToState(state);
        command2.ApplyToState(state);

        // Assert - Should have the latest values
        Assert.That(state.Info.FairlightMixer?.Inputs, Is.EqualTo(24));
        Assert.That(state.Info.FairlightMixer?.Monitors, Is.EqualTo(4));
        Assert.That(state.Fairlight, Is.Not.Null);
        Assert.That(state.Fairlight.Inputs, Is.Empty);
    }
}