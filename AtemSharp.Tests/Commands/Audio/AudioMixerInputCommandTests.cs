using AtemSharp.Commands.Audio;
using AtemSharp.Enums.Audio;
using AtemSharp.State;
using AtemSharp.State.Audio.ClassicAudio;

namespace AtemSharp.Tests.Commands.Audio;

[TestFixture]
public class AudioMixerInputCommandTests : SerializedCommandTestBase<AudioMixerInputCommand,
	AudioMixerInputCommandTests.CommandData>
{
	protected override Range[] GetFloatingPointByteRanges()
	{
		return
		[
			6..8, // bytes 6-7 for Gain
			8..10  // bytes 8-9 for Balance
		];
	}

	public class CommandData : CommandDataBase
	{
		public ushort Index { get; set; }
		public AudioMixOption MixOption { get; set; }
		public double Gain { get; set; }
		public double Balance { get; set; }
		public bool RcaToXlrEnabled { get; set; }
	}

	protected override AudioMixerInputCommand CreateSut(TestCaseData testCase)
	{
		// Create state with the required audio channel
		var state = CreateStateWithAudioChannel(testCase.Command.Index);

		// Create command with the index
		var command = new AudioMixerInputCommand(testCase.Command.Index, state);

		// Set the actual values that should be written
		command.MixOption = testCase.Command.MixOption;
		command.Gain = testCase.Command.Gain;
		command.Balance = testCase.Command.Balance;
		command.RcaToXlrEnabled = testCase.Command.RcaToXlrEnabled;

		return command;
	}

	/// <summary>
	/// Creates an AtemState with a valid audio channel at the specified index
	/// </summary>
	private static AtemState CreateStateWithAudioChannel(ushort index)
	{
		var state = new AtemState
		{
			Audio = new ClassicAudioState
			{
				Channels = new Dictionary<int, ClassicAudioChannel>
				{
					[index] = new ClassicAudioChannel
					{
						MixOption = AudioMixOption.Off,
						Gain = 0.0,
						Balance = 0.0,
						RcaToXlrEnabled = false
					}
				}
			}
		};
		return state;
	}

	[Test]
	public void Constructor_WithValidIndex_ShouldInitializeCorrectly()
	{
		// Arrange
		const ushort index = 5;
		var state = CreateStateWithAudioChannel(index);

		// Act
		var command = new AudioMixerInputCommand(index, state);

		// Assert
		Assert.That(command.MixOption, Is.EqualTo(AudioMixOption.Off));
		Assert.That(command.Gain, Is.EqualTo(0.0));
		Assert.That(command.Balance, Is.EqualTo(0.0));
		Assert.That(command.RcaToXlrEnabled, Is.EqualTo(false));
	}

	[Test]
	public void Constructor_WithNullAudioState_ShouldThrowInvalidIdError()
	{
		// Arrange
		const ushort index = 1;
		var state = new AtemState(); // No audio state

		// Act & Assert
		var ex = Assert.Throws<InvalidOperationException>(() => new AudioMixerInputCommand(index, state));
	}

	[Test]
	public void Constructor_WithNoChannels_ShouldThrowInvalidIdError()
	{
		// Arrange
		const ushort index = 1;
		var state = new AtemState
		{
			Audio = new ClassicAudioState() // Channels collection is empty
		};

		// Act & Assert
		Assert.Throws<IndexOutOfRangeException>(() => new AudioMixerInputCommand(index, state));
	}

	[Test]
	public void Constructor_WithMissingChannelIndex_ShouldThrowInvalidIdError()
	{
		// Arrange
		const ushort index = 5;
		var state = new AtemState
		{
			Audio = new ClassicAudioState
			{
				Channels = new Dictionary<int, ClassicAudioChannel>
				{
					[1] = new ClassicAudioChannel(), // Different index
					[2] = new ClassicAudioChannel()
				}
			}
		};

		// Act & Assert
		Assert.Throws<IndexOutOfRangeException>(() => new AudioMixerInputCommand(index, state));
	}

	[TestCase(-60.0)]
	[TestCase(-30.0)]
	[TestCase(0.0)]
	[TestCase(3.5)]
	[TestCase(6.0)]
	public void GainProperty_WithValidValues_ShouldSetCorrectly(double validGain)
	{
		// Arrange
		var state = CreateStateWithAudioChannel(1);
		var command = new AudioMixerInputCommand(1, state);

		// Act
		command.Gain = validGain;

		// Assert
		Assert.That(command.Gain, Is.EqualTo(validGain));
		Assert.That(command.Flag & (1 << 1), Is.Not.EqualTo(0), "Gain flag should be set");
	}


	[TestCase(-50.0)]
	[TestCase(-25.0)]
	[TestCase(0.0)]
	[TestCase(25.0)]
	[TestCase(50.0)]
	public void BalanceProperty_WithValidValues_ShouldSetCorrectly(double validBalance)
	{
		// Arrange
		var state = CreateStateWithAudioChannel(1);
		var command = new AudioMixerInputCommand(1, state);

		// Act
		command.Balance = validBalance;

		// Assert
		Assert.That(command.Balance, Is.EqualTo(validBalance));
		Assert.That(command.Flag & (1 << 2), Is.Not.EqualTo(0), "Balance flag should be set");
	}

	[TestCase(AudioMixOption.Off)]
	[TestCase(AudioMixOption.On)]
	[TestCase(AudioMixOption.AudioFollowVideo)]
	public void MixOptionProperty_WithValidValues_ShouldSetCorrectlyAndSetFlag(AudioMixOption mixOption)
	{
		// Arrange
		var state = CreateStateWithAudioChannel(1);
		var command = new AudioMixerInputCommand(1, state);

		// Act
		command.MixOption = mixOption;

		// Assert
		Assert.That(command.MixOption, Is.EqualTo(mixOption));
		Assert.That(command.Flag & (1 << 0), Is.Not.EqualTo(0), "MixOption flag should be set");
	}

	[TestCase(true)]
	[TestCase(false)]
	public void RcaToXlrEnabledProperty_ShouldSetCorrectlyAndSetFlag(bool enabled)
	{
		// Arrange
		var state = CreateStateWithAudioChannel(1);
		var command = new AudioMixerInputCommand(1, state);

		// Act
		command.RcaToXlrEnabled = enabled;

		// Assert
		Assert.That(command.RcaToXlrEnabled, Is.EqualTo(enabled));
		Assert.That(command.Flag & (1 << 3), Is.Not.EqualTo(0), "RcaToXlrEnabled flag should be set");
	}

	[Test]
	public void Constructor_ShouldInitializeFromStateWithoutSettingFlags()
	{
		// Arrange
		const ushort index = 3;
		var state = new AtemState
		{
			Audio = new ClassicAudioState
			{
				Channels = new Dictionary<int, ClassicAudioChannel>
				{
					[index] = new ClassicAudioChannel
					{
						MixOption = AudioMixOption.On,
						Gain = -12.5,
						Balance = 15.0,
						RcaToXlrEnabled = true
					}
				}
			}
		};

		// Act
		var command = new AudioMixerInputCommand(index, state);

		// Assert
		Assert.That(command.MixOption, Is.EqualTo(AudioMixOption.On));
		Assert.That(command.Gain, Is.EqualTo(-12.5));
		Assert.That(command.Balance, Is.EqualTo(15.0));
		Assert.That(command.RcaToXlrEnabled, Is.EqualTo(true));
		Assert.That(command.Flag, Is.EqualTo(0), "No flags should be set when initializing from state");
	}

	[Test]
	public void SetMultipleProperties_ShouldSetCombinedFlags()
	{
		// Arrange
		var state = CreateStateWithAudioChannel(1);
		var command = new AudioMixerInputCommand(1, state);

		// Act
		command.MixOption = AudioMixOption.On;        // Flag 0x01
		command.Gain = -10.0;                         // Flag 0x02
		command.RcaToXlrEnabled = true;              // Flag 0x08

		// Assert
		Assert.That(command.Flag, Is.EqualTo(0x01 | 0x02 | 0x08));
	}
}
