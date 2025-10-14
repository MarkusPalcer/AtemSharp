using AtemSharp.Commands.Audio;
using AtemSharp.Enums;
using AtemSharp.State;
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
			Audio = new AudioState
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
		Assert.That(state.Audio.Master.Gain, Is.EqualTo(-12.5));
		Assert.That(state.Audio.Master.Balance, Is.EqualTo(25.0));
		Assert.That(state.Audio.Master.FollowFadeToBlack, Is.True);
	}

	[Test]
	public void ApplyToState_WithNullAudioState_ShouldThrowInvalidIdError()
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
		var ex = Assert.Throws<InvalidIdError>(() => command.ApplyToState(state));
		Assert.That(ex.Message, Does.Contain("Classic Audio"));
		Assert.That(ex.Message, Does.Contain("master"));
	}

	[Test]
	public void ApplyToState_WithNullMasterChannel_ShouldCreateAndUpdateMasterChannel()
	{
		// Arrange
		var state = new AtemState
		{
			Audio = new AudioState() // Audio state exists but no master channel
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
		Assert.That(state.Audio.Master, Is.Not.Null);
		Assert.That(state.Audio.Master.Gain, Is.EqualTo(-8.0));
		Assert.That(state.Audio.Master.Balance, Is.EqualTo(15.0));
		Assert.That(state.Audio.Master.FollowFadeToBlack, Is.True);
	}

	[Test]
	public void Deserialize_WithValidData_ShouldCreateCommandWithCorrectProperties()
	{
		// Arrange - Create test data matching the AMMO format
		var testData = new byte[]
		{
			0x32, 0xEC, // Gain as UInt16 (corresponds to -8.006111663332227 dB)
			0x10, 0xD6, // Balance as Int16 (corresponds to 21.548807421489055)
			0x01        // FollowFadeToBlack as byte (true)
		};

		using var stream = new MemoryStream(testData);

		// Act
		var command = AudioMixerMasterUpdateCommand.Deserialize(stream, ProtocolVersion.V7_2);

		// Assert - Use exact values from the test data JSON
		Assert.That(command.Gain, Is.EqualTo(-8.006111663332227).Within(0.01));
		Assert.That(command.Balance, Is.EqualTo(21.548807421489055).Within(0.01));
		Assert.That(command.FollowFadeToBlack, Is.True);
	}

	[Test]
	public void Deserialize_WithFalseFollowFadeToBlack_ShouldSetPropertyCorrectly()
	{
		// Arrange
		var testData = new byte[]
		{
			0x2F, 0xE4, // Gain as UInt16 (corresponds to -8.539189295546434 dB)
			0xE4, 0xCA, // Balance as Int16 (corresponds to -34.829096815003595)
			0x00        // FollowFadeToBlack as byte (false)
		};

		using var stream = new MemoryStream(testData);

		// Act
		var command = AudioMixerMasterUpdateCommand.Deserialize(stream, ProtocolVersion.V7_2);

		// Assert
		Assert.That(command.Gain, Is.EqualTo(-8.539189295546434).Within(0.01));
		Assert.That(command.Balance, Is.EqualTo(-34.829096815003595).Within(0.01));
		Assert.That(command.FollowFadeToBlack, Is.False);
	}

	[Test]
	public void ApplyToState_ValidAudioState_ShouldSucceed()
	{
		// Arrange
		var state = new AtemState();
		state.Audio = new AudioState();

		var command = new AudioMixerMasterUpdateCommand
		{
			Gain = -10.0,
			Balance = 0.0,
			FollowFadeToBlack = false
		};

		// Act & Assert
		Assert.DoesNotThrow(() => command.ApplyToState(state));
		Assert.That(state.Audio.Master, Is.Not.Null);
		Assert.That(state.Audio.Master.Gain, Is.EqualTo(-10.0));
	}

	[Test]
	public void ApplyToState_NullAudioState_ShouldThrowInvalidIdError()
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
		var ex = Assert.Throws<InvalidIdError>(() => command.ApplyToState(state));
		Assert.That(ex.Message, Contains.Substring("Classic Audio"));
	}
}
