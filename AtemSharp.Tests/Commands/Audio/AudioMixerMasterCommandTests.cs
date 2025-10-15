using AtemSharp.Commands.Audio;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Audio;

[TestFixture]
public class AudioMixerMasterCommandTests : SerializedCommandTestBase<AudioMixerMasterCommand,
	AudioMixerMasterCommandTests.CommandData>
{
	protected override Range[] GetFloatingPointByteRanges()
	{
		return
		[
			2..4, // bytes 2-3 for Gain
			4..6  // bytes 4-5 for Balance
		];
	}

	public class CommandData : CommandDataBase
	{
		public double Gain { get; set; }
		public double Balance { get; set; }
		public bool FollowFadeToBlack { get; set; }
	}

	protected override AudioMixerMasterCommand CreateSut(TestCaseData testCase)
	{
		// Create state with the required audio master channel
		var state = CreateStateWithAudioMaster();

		// Create command
		var command = new AudioMixerMasterCommand(state);

		// Set the actual values that should be written
		command.Gain = testCase.Command.Gain;
		command.Balance = testCase.Command.Balance;
		command.FollowFadeToBlack = testCase.Command.FollowFadeToBlack;

		return command;
	}

	/// <summary>
	/// Creates an AtemState with a valid audio master channel
	/// </summary>
	private static AtemState CreateStateWithAudioMaster()
	{
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
		return state;
	}

	[Test]
	public void Constructor_WithValidState_ShouldInitializeCorrectly()
	{
		// Arrange
		var state = CreateStateWithAudioMaster();

		// Act
		var command = new AudioMixerMasterCommand(state);

		// Assert
		Assert.That(command.Gain, Is.EqualTo(0.0));
		Assert.That(command.Balance, Is.EqualTo(0.0));
		Assert.That(command.FollowFadeToBlack, Is.EqualTo(false));
	}

	[Test]
	public void Constructor_WithNullAudioState_ShouldThrowInvalidIdError()
	{
		// Arrange
		var state = new AtemState(); // No audio state

		// Act & Assert
		Assert.Throws<InvalidOperationException>(() => new AudioMixerMasterCommand(state));
	}

	[Test]
	public void Constructor_WithNullAudioMaster_ShouldThrowInvalidIdError()
	{
		// Arrange
		var state = new AtemState
		{
			Audio = new ClassicAudioState() // Audio state exists but no master channel
		};

		// Act & Assert
		Assert.Throws<InvalidOperationException>(() => new AudioMixerMasterCommand(state));
	}

	[Test]
	public void SetGain_WithValidValue_ShouldSetPropertyAndFlag()
	{
		// Arrange
		var state = CreateStateWithAudioMaster();
		var command = new AudioMixerMasterCommand(state);
		const double expectedGain = -12.5;

		// Act
		command.Gain = expectedGain;

		// Assert
		Assert.That(command.Gain, Is.EqualTo(expectedGain));
		Assert.That(command.Flag & (1 << 0), Is.Not.EqualTo(0), "Gain flag should be set");
	}

	[Test]
	public void SetGain_WithInvalidValueTooLow_ShouldThrowArgumentOutOfRangeException()
	{
		// Arrange
		var state = CreateStateWithAudioMaster();
		var command = new AudioMixerMasterCommand(state);

		// Act & Assert
		var ex = Assert.Throws<ArgumentOutOfRangeException>(() => command.Gain = -70.0);
		Assert.That(ex.ParamName, Is.EqualTo("value"));
		Assert.That(ex.Message, Does.Contain("must be between -60.0 and +6.0"));
	}

	[Test]
	public void SetGain_WithInvalidValueTooHigh_ShouldThrowArgumentOutOfRangeException()
	{
		// Arrange
		var state = CreateStateWithAudioMaster();
		var command = new AudioMixerMasterCommand(state);

		// Act & Assert
		var ex = Assert.Throws<ArgumentOutOfRangeException>(() => command.Gain = 10.0);
		Assert.That(ex.ParamName, Is.EqualTo("value"));
		Assert.That(ex.Message, Does.Contain("must be between -60.0 and +6.0"));
	}

	[Test]
	public void SetBalance_WithValidValue_ShouldSetPropertyAndFlag()
	{
		// Arrange
		var state = CreateStateWithAudioMaster();
		var command = new AudioMixerMasterCommand(state);
		const double expectedBalance = 25.0;

		// Act
		command.Balance = expectedBalance;

		// Assert
		Assert.That(command.Balance, Is.EqualTo(expectedBalance));
		Assert.That(command.Flag & (1 << 1), Is.Not.EqualTo(0), "Balance flag should be set");
	}

	[Test]
	public void SetBalance_WithInvalidValueTooLow_ShouldThrowArgumentOutOfRangeException()
	{
		// Arrange
		var state = CreateStateWithAudioMaster();
		var command = new AudioMixerMasterCommand(state);

		// Act & Assert
		var ex = Assert.Throws<ArgumentOutOfRangeException>(() => command.Balance = -60.0);
		Assert.That(ex.ParamName, Is.EqualTo("value"));
		Assert.That(ex.Message, Does.Contain("must be between -50.0 and +50.0"));
	}

	[Test]
	public void SetBalance_WithInvalidValueTooHigh_ShouldThrowArgumentOutOfRangeException()
	{
		// Arrange
		var state = CreateStateWithAudioMaster();
		var command = new AudioMixerMasterCommand(state);

		// Act & Assert
		var ex = Assert.Throws<ArgumentOutOfRangeException>(() => command.Balance = 60.0);
		Assert.That(ex.ParamName, Is.EqualTo("value"));
		Assert.That(ex.Message, Does.Contain("must be between -50.0 and +50.0"));
	}

	[Test]
	public void SetFollowFadeToBlack_WithValue_ShouldSetPropertyAndFlag()
	{
		// Arrange
		var state = CreateStateWithAudioMaster();
		var command = new AudioMixerMasterCommand(state);

		// Act
		command.FollowFadeToBlack = true;

		// Assert
		Assert.That(command.FollowFadeToBlack, Is.True);
		Assert.That(command.Flag & (1 << 2), Is.Not.EqualTo(0), "FollowFadeToBlack flag should be set");
	}

	[Test]
	public void SetMultipleProperties_ShouldSetMultipleFlags()
	{
		// Arrange
		var state = CreateStateWithAudioMaster();
		var command = new AudioMixerMasterCommand(state);

		// Act
		command.Gain = -5.0;
		command.Balance = 10.0;
		command.FollowFadeToBlack = true;

		// Assert
		Assert.That(command.Flag & (1 << 0), Is.Not.EqualTo(0), "Gain flag should be set");
		Assert.That(command.Flag & (1 << 1), Is.Not.EqualTo(0), "Balance flag should be set");
		Assert.That(command.Flag & (1 << 2), Is.Not.EqualTo(0), "FollowFadeToBlack flag should be set");
		Assert.That(command.Flag, Is.EqualTo(0x07), "All three flags should be set");
	}
}
