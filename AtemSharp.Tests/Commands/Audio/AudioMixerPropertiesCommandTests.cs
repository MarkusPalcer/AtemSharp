using AtemSharp.Commands.Audio;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Audio;

[TestFixture]
public class AudioMixerPropertiesCommandTests : SerializedCommandTestBase<AudioMixerPropertiesCommand,
	AudioMixerPropertiesCommandTests.CommandData>
{
	public class CommandData : CommandDataBase
	{
		public bool AudioFollowVideo { get; set; }
	}

	protected override AudioMixerPropertiesCommand CreateSut(TestCaseData testCase)
	{
		// Create state with the required audio state
		var state = CreateStateWithAudio();
		
		// Create command
		var command = new AudioMixerPropertiesCommand(state);

		// Set the actual values that should be written
		command.AudioFollowVideo = testCase.Command.AudioFollowVideo;
		
		return command;
	}

	/// <summary>
	/// Creates an AtemState with a valid audio state
	/// </summary>
	private static AtemState CreateStateWithAudio()
	{
		var state = new AtemState
		{
			Audio = new AudioState
			{
				AudioFollowVideoCrossfadeTransitionEnabled = false
			}
		};
		return state;
	}

	[Test]
	public void Constructor_WithValidState_ShouldInitializeCorrectly()
	{
		// Arrange
		var state = CreateStateWithAudio();

		// Act
		var command = new AudioMixerPropertiesCommand(state);

		// Assert
		Assert.That(command.AudioFollowVideo, Is.EqualTo(false));
	}

	[Test]
	public void Constructor_WithNullAudioState_ShouldThrowInvalidIdError()
	{
		// Arrange
		var state = new AtemState(); // No audio state

		// Act & Assert
		var ex = Assert.Throws<InvalidIdError>(() => new AudioMixerPropertiesCommand(state));
		Assert.That(ex.Message, Does.Contain("Classic Audio"));
	}

	[Test]
	public void AudioFollowVideo_WhenSet_ShouldSetFlagBit0()
	{
		// Arrange
		var state = CreateStateWithAudio();
		var command = new AudioMixerPropertiesCommand(state);

		// Act
		command.AudioFollowVideo = true;

		// Assert
		Assert.That(command.Flag & (1 << 0), Is.Not.EqualTo(0), "Flag bit 0 should be set");
		Assert.That(command.AudioFollowVideo, Is.True);
	}

	[Test]
	public void AudioFollowVideo_WithDifferentValues_ShouldSetCorrectFlag()
	{
		// Arrange
		var state = CreateStateWithAudio();

		// Test false value
		var command1 = new AudioMixerPropertiesCommand(state);
		command1.AudioFollowVideo = false;
		Assert.That(command1.Flag & (1 << 0), Is.Not.EqualTo(0), "Flag bit 0 should be set for false");
		Assert.That(command1.AudioFollowVideo, Is.False);

		// Test true value
		var command2 = new AudioMixerPropertiesCommand(state);
		command2.AudioFollowVideo = true;
		Assert.That(command2.Flag & (1 << 0), Is.Not.EqualTo(0), "Flag bit 0 should be set for true");
		Assert.That(command2.AudioFollowVideo, Is.True);
	}

	[Test]
	public void Serialize_WithAudioFollowVideoTrue_ShouldGenerateCorrectBytes()
	{
		// Arrange
		var state = CreateStateWithAudio();
		var command = new AudioMixerPropertiesCommand(state);
		command.AudioFollowVideo = true;

		// Act
		var result = command.Serialize(ProtocolVersion.V8_0);

		// Assert
		Assert.That(result.Length, Is.EqualTo(4));
		Assert.That(result[0], Is.EqualTo(0x01)); // Flag: bit 0 set
		Assert.That(result[1], Is.EqualTo(0x01)); // AudioFollowVideo: true
		Assert.That(result[2], Is.EqualTo(0x00)); // Padding
		Assert.That(result[3], Is.EqualTo(0x00)); // Padding
	}

	[Test]
	public void Serialize_WithAudioFollowVideoFalse_ShouldGenerateCorrectBytes()
	{
		// Arrange
		var state = CreateStateWithAudio();
		var command = new AudioMixerPropertiesCommand(state);
		command.AudioFollowVideo = false;

		// Act
		var result = command.Serialize(ProtocolVersion.V8_0);

		// Assert
		Assert.That(result.Length, Is.EqualTo(4));
		Assert.That(result[0], Is.EqualTo(0x01)); // Flag: bit 0 set
		Assert.That(result[1], Is.EqualTo(0x00)); // AudioFollowVideo: false
		Assert.That(result[2], Is.EqualTo(0x00)); // Padding
		Assert.That(result[3], Is.EqualTo(0x00)); // Padding
	}
}