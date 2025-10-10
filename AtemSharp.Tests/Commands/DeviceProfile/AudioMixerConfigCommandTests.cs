using AtemSharp.Commands.DeviceProfile;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.DeviceProfile;

[TestFixture]
public class AudioMixerConfigCommandTests : DeserializedCommandTestBase<AudioMixerConfigCommand, AudioMixerConfigCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Inputs { get; set; }
        public byte Monitors { get; set; }
        public byte Headphones { get; set; }
    }

    protected override void CompareCommandProperties(AudioMixerConfigCommand actualCommand, CommandData expectedData, TestCaseData testCase)
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

        // Compare Headphones
        if (actualCommand.Headphones != expectedData.Headphones)
        {
            failures.Add($"Headphones: expected {expectedData.Headphones}, actual {actualCommand.Headphones}");
        }

        if (failures.Count > 0)
        {
            Assert.Fail($"Command property comparison failed:\n{string.Join("\n", failures)}");
        }
    }

    [Test]
    public void ApplyToState_WithValidData_ShouldUpdateInfoAndAudioState()
    {
        // Arrange
        var state = new AtemState();
        var command = new AudioMixerConfigCommand
        {
            Inputs = 20,
            Monitors = 2,
            Headphones = 1
        };

        // Act
        var result = command.ApplyToState(state);

        // Assert
        Assert.That(state.Info.AudioMixer, Is.Not.Null);
        Assert.That(state.Info.AudioMixer.Inputs, Is.EqualTo(20));
        Assert.That(state.Info.AudioMixer.Monitors, Is.EqualTo(2));
        Assert.That(state.Info.AudioMixer.Headphones, Is.EqualTo(1));
        
        Assert.That(state.Audio, Is.Not.Null);
        Assert.That(state.Audio.Channels, Is.Not.Null);
        Assert.That(state.Audio.Channels, Is.Empty);
        
        Assert.That(result, Is.EqualTo(new[] { "info.audioMixer", "audio" }));
    }

    [Test]
    public void Deserialize_WithTypicalValues_ShouldDeserializeCorrectly()
    {
        // Arrange
        var data = new byte[] { 0x14, 0x02, 0x01 }; // Inputs: 20, Monitors: 2, Headphones: 1
        using var stream = new MemoryStream(data);

        // Act
        var command = AudioMixerConfigCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert
        Assert.That(command.Inputs, Is.EqualTo(20));
        Assert.That(command.Monitors, Is.EqualTo(2));
        Assert.That(command.Headphones, Is.EqualTo(1));
    }

    [Test]
    public void Deserialize_WithZeroValues_ShouldDeserializeCorrectly()
    {
        // Arrange
        var data = new byte[] { 0x00, 0x00, 0x00 }; // All zeros
        using var stream = new MemoryStream(data);

        // Act
        var command = AudioMixerConfigCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert
        Assert.That(command.Inputs, Is.EqualTo(0));
        Assert.That(command.Monitors, Is.EqualTo(0));
        Assert.That(command.Headphones, Is.EqualTo(0));
    }

    [Test]
    public void Deserialize_WithMaxValues_ShouldDeserializeCorrectly()
    {
        // Arrange
        var data = new byte[] { 0xFF, 0xFF, 0xFF }; // All max byte values
        using var stream = new MemoryStream(data);

        // Act
        var command = AudioMixerConfigCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert
        Assert.That(command.Inputs, Is.EqualTo(255));
        Assert.That(command.Monitors, Is.EqualTo(255));
        Assert.That(command.Headphones, Is.EqualTo(255));
    }

    [Test]
    public void ApplyToState_MultipleCalls_ShouldReplaceAudioMixerInfo()
    {
        // Arrange
        var state = new AtemState();
        
        var firstCommand = new AudioMixerConfigCommand
        {
            Inputs = 10,
            Monitors = 1,
            Headphones = 1
        };
        
        var secondCommand = new AudioMixerConfigCommand
        {
            Inputs = 20,
            Monitors = 2,
            Headphones = 1
        };

        // Act
        firstCommand.ApplyToState(state);
        var result = secondCommand.ApplyToState(state);

        // Assert - should have the values from the second command
        Assert.That(state.Info.AudioMixer, Is.Not.Null);
        Assert.That(state.Info.AudioMixer!.Inputs, Is.EqualTo(20));
        Assert.That(state.Info.AudioMixer.Monitors, Is.EqualTo(2));
        Assert.That(state.Info.AudioMixer.Headphones, Is.EqualTo(1));
        
        Assert.That(result, Is.EqualTo(new[] { "info.audioMixer", "audio" }));
    }
}