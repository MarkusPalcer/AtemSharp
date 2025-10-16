using AtemSharp.Commands.Audio;
using AtemSharp.State;
using AtemSharp.State.Audio.ClassicAudio;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests.Commands.Audio;

[TestFixture]
public class AudioMixerMasterUpdateCommandTests : DeserializedCommandTestBase<AudioMixerMasterUpdateCommand,
	AudioMixerMasterUpdateCommandTests.CommandData>
{
	public class CommandData : CommandDataBase
	{
		public double Gain { get; set; }
		public double Balance { get; set; }
		public bool FollowFadeToBlack { get; set; }
	}

	protected override void CompareCommandProperties(AudioMixerMasterUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
	{
		var failures = new List<string>();

		// Compare Gain (floating point)
		if (!Utilities.AreApproximatelyEqual(actualCommand.Gain, expectedData.Gain))
		{
			failures.Add($"Gain: expected {expectedData.Gain}, actual {actualCommand.Gain}");
		}

		// Compare Balance (floating point)
		if (!Utilities.AreApproximatelyEqual(actualCommand.Balance, expectedData.Balance))
		{
			failures.Add($"Balance: expected {expectedData.Balance}, actual {actualCommand.Balance}");
		}

		// Compare FollowFadeToBlack (boolean)
		if (actualCommand.FollowFadeToBlack != expectedData.FollowFadeToBlack)
		{
			failures.Add($"FollowFadeToBlack: expected {expectedData.FollowFadeToBlack}, actual {actualCommand.FollowFadeToBlack}");
		}

		if (failures.Count > 0)
		{
			Assert.Fail($"Command property comparison failed:\n{string.Join("\n", failures)}");
		}
	}

	[Test]
	public void ApplyToState_WithValidState_ShouldUpdateMasterChannel()
	{
		// Arrange
		var state = new AtemState
		{
			Audio = new ClassicAudioState
			{
				Master = new ClassicAudioMasterChannel
				{
					Gain = 0.0,
					Balance = 0.0,
					FollowFadeToBlack = false
				}
			}
		};

		var command = new AudioMixerMasterUpdateCommand
		{
			Gain = -12.5,
			Balance = 25.0,
			FollowFadeToBlack = true
		};

		// Act
		command.ApplyToState(state);

		// Assert
		Assert.That(state.Audio.As<ClassicAudioState>().Master!.Gain, Is.EqualTo(-12.5));
		Assert.That(state.Audio.As<ClassicAudioState>().Master!.Balance, Is.EqualTo(25.0));
		Assert.That(state.Audio.As<ClassicAudioState>().Master!.FollowFadeToBlack, Is.True);
	}

	[Test]
	public void ApplyToState_WithNullAudioState_ShouldThrow()
	{
		// Arrange
		var state = new AtemState(); // No audio state
		var command = new AudioMixerMasterUpdateCommand
		{
			Gain = -5.0,
			Balance = 0.0,
			FollowFadeToBlack = false
		};

		// Act & Assert
		Assert.Throws<InvalidOperationException>(() => command.ApplyToState(state));
	}

	[Test]
	public void ApplyToState_WithNullMasterChannel_ShouldCreateAndUpdateMasterChannel()
	{
		// Arrange
		var state = new AtemState
		{
			Audio = new ClassicAudioState() // Audio state exists but no master channel
		};

		var command = new AudioMixerMasterUpdateCommand
		{
			Gain = -8.0,
			Balance = 15.0,
			FollowFadeToBlack = true
		};

		// Act
		command.ApplyToState(state);

		// Assert
		Assert.That(state.Audio.As<ClassicAudioState>().Master, Is.Not.Null);
		Assert.That(state.Audio.As<ClassicAudioState>().Master.Gain, Is.EqualTo(-8.0));
		Assert.That(state.Audio.As<ClassicAudioState>().Master.Balance, Is.EqualTo(15.0));
		Assert.That(state.Audio.As<ClassicAudioState>().Master.FollowFadeToBlack, Is.True);
	}

	[Test]
	public void ApplyToState_ValidAudioState_ShouldSucceed()
	{
		// Arrange
		var state = new AtemState();
		state.Audio = new ClassicAudioState();

		var command = new AudioMixerMasterUpdateCommand
		{
			Gain = -10.0,
			Balance = 0.0,
			FollowFadeToBlack = false
		};

		// Act & Assert
		Assert.DoesNotThrow(() => command.ApplyToState(state));
		Assert.That(state.Audio.As<ClassicAudioState>().Master, Is.Not.Null);
		Assert.That(state.Audio.As<ClassicAudioState>().Master.Gain, Is.EqualTo(-10.0));
	}

	[Test]
	public void ApplyToState_NullAudioState_ShouldThrow()
	{
		// Arrange
		var state = new AtemState();
		state.Audio = null; // Audio subsystem not available

		var command = new AudioMixerMasterUpdateCommand
		{
			Gain = -10.0,
			Balance = 0.0,
			FollowFadeToBlack = false
		};

		// Act & Assert
		Assert.Throws<InvalidOperationException>(() => command.ApplyToState(state));
	}
}
