using AtemSharp.Commands.Audio;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Audio;

[TestFixture]
public class AudioMixerPropertiesUpdateCommandTests : DeserializedCommandTestBase<AudioMixerPropertiesUpdateCommand,
	AudioMixerPropertiesUpdateCommandTests.CommandData>
{
	public class CommandData : CommandDataBase
	{
		public bool AudioFollowVideo { get; set; }
	}

	protected override void CompareCommandProperties(AudioMixerPropertiesUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
	{
		var failures = new List<string>();

		// Compare AudioFollowVideo (boolean)
		if (actualCommand.AudioFollowVideo != expectedData.AudioFollowVideo)
		{
			failures.Add($"AudioFollowVideo: expected {expectedData.AudioFollowVideo}, actual {actualCommand.AudioFollowVideo}");
		}

		if (failures.Count > 0)
		{
			Assert.Fail($"Command property comparison failed:\n{string.Join("\n", failures)}");
		}
	}

	[Test]
	public void ApplyToState_WithValidState_ShouldUpdateAudioFollowVideoProperty()
	{
		// Arrange
		var state = new AtemState
		{
			Audio = new AudioState
			{
				AudioFollowVideoCrossfadeTransitionEnabled = false
			}
		};

		var command = new AudioMixerPropertiesUpdateCommand
		{
			AudioFollowVideo = true
		};

		// Act
		command.ApplyToState(state);

		// Assert
		Assert.That(state.Audio.AudioFollowVideoCrossfadeTransitionEnabled, Is.True);
	}

	[Test]
	public void ApplyToState_WithNullAudioState_ShouldThrowInvalidIdError()
	{
		// Arrange
		var state = new AtemState(); // No audio state
		var command = new AudioMixerPropertiesUpdateCommand
		{
			AudioFollowVideo = true
		};

		// Act & Assert
		var ex = Assert.Throws<InvalidIdError>(() => command.ApplyToState(state));
		Assert.That(ex.Message, Does.Contain("Classic Audio"));
	}

	[Test]
	public void Deserialize_WithAudioFollowVideoTrue_ShouldDeserializeCorrectly()
	{
		// Arrange
		var data = new byte[] { 0x01 }; // AudioFollowVideo: true
		using var stream = new MemoryStream(data);

		// Act
		var command = AudioMixerPropertiesUpdateCommand.Deserialize(stream, ProtocolVersion.V7_2);

		// Assert
		Assert.That(command.AudioFollowVideo, Is.True);
	}

	[Test]
	public void Deserialize_WithAudioFollowVideoFalse_ShouldDeserializeCorrectly()
	{
		// Arrange
		var data = new byte[] { 0x00 }; // AudioFollowVideo: false
		using var stream = new MemoryStream(data);

		// Act
		var command = AudioMixerPropertiesUpdateCommand.Deserialize(stream, ProtocolVersion.V7_2);

		// Assert
		Assert.That(command.AudioFollowVideo, Is.False);
	}

	[Test]
	public void ApplyToState_WithDifferentValues_ShouldUpdateCorrectly()
	{
		// Test true value
		var state1 = new AtemState { Audio = new AudioState() };
		var command1 = new AudioMixerPropertiesUpdateCommand { AudioFollowVideo = true };

		command1.ApplyToState(state1);
		Assert.That(state1.Audio.AudioFollowVideoCrossfadeTransitionEnabled, Is.True);

		// Test false value
		var state2 = new AtemState { Audio = new AudioState() };
		var command2 = new AudioMixerPropertiesUpdateCommand { AudioFollowVideo = false };

		command2.ApplyToState(state2);
		Assert.That(state2.Audio.AudioFollowVideoCrossfadeTransitionEnabled, Is.False);
	}
}
