using AtemSharp.Commands.Audio;
using AtemSharp.State;
using AtemSharp.Tests.TestUtilities;

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
			Audio = new ClassicAudioState
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
		Assert.That(state.Audio.As<ClassicAudioState>().AudioFollowsVideo, Is.True);
	}

	[Test]
	public void ApplyToState_WithNullAudioState_ShouldThrow()
	{
		// Arrange
		var state = new AtemState(); // No audio state
		var command = new AudioMixerPropertiesUpdateCommand
		{
			AudioFollowVideo = true
		};

		// Act & Assert
        Assert.Throws<InvalidOperationException>(() => command.ApplyToState(state));
	}

	[Test]
	public void ApplyToState_WithDifferentValues_ShouldUpdateCorrectly()
	{
		// Test true value
		var state1 = new AtemState { Audio = new ClassicAudioState() };
		var command1 = new AudioMixerPropertiesUpdateCommand { AudioFollowVideo = true };

		command1.ApplyToState(state1);
		Assert.That(state1.Audio.As<ClassicAudioState>().AudioFollowsVideo, Is.True);

		// Test false value
		var state2 = new AtemState { Audio = new ClassicAudioState() };
		var command2 = new AudioMixerPropertiesUpdateCommand { AudioFollowVideo = false };

		command2.ApplyToState(state2);
		Assert.That(state2.Audio.As<ClassicAudioState>().AudioFollowsVideo, Is.False);
	}
}
