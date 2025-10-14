using AtemSharp.Commands.Audio;
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
				AudioFollowsVideo = false
			}
		};

		var command = new AudioMixerPropertiesUpdateCommand
		{
			AudioFollowVideo = true
		};

		// Act
		command.ApplyToState(state);

		// Assert
		Assert.That(state.Audio.AudioFollowsVideo, Is.True);
	}

	[Test]
	public void ApplyToState_WithNullAudioState_ShouldCreateAudioState()
	{
		// Arrange
		var state = new AtemState(); // No audio state
		var command = new AudioMixerPropertiesUpdateCommand
		{
			AudioFollowVideo = true
		};

		// Act & Assert
        command.ApplyToState(state);
        Assert.That(state.Audio, Is.Not.Null);
        Assert.That(state.Audio.AudioFollowsVideo, Is.True);
	}

	[Test]
	public void ApplyToState_WithDifferentValues_ShouldUpdateCorrectly()
	{
		// Test true value
		var state1 = new AtemState { Audio = new AudioState() };
		var command1 = new AudioMixerPropertiesUpdateCommand { AudioFollowVideo = true };

		command1.ApplyToState(state1);
		Assert.That(state1.Audio.AudioFollowsVideo, Is.True);

		// Test false value
		var state2 = new AtemState { Audio = new AudioState() };
		var command2 = new AudioMixerPropertiesUpdateCommand { AudioFollowVideo = false };

		command2.ApplyToState(state2);
		Assert.That(state2.Audio.AudioFollowsVideo, Is.False);
	}
}
