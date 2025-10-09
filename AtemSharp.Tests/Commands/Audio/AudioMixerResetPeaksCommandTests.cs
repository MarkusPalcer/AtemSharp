using AtemSharp.Commands.Audio;
using AtemSharp.Enums;

namespace AtemSharp.Tests.Commands.Audio;

[TestFixture]
public class AudioMixerResetPeaksCommandTests : SerializedCommandTestBase<AudioMixerResetPeaksCommand,
	AudioMixerResetPeaksCommandTests.CommandData>
{
	public class CommandData : CommandDataBase
	{
		public bool All { get; set; }
		public ushort Input { get; set; }
		public bool Master { get; set; }
		public bool Monitor { get; set; }
	}

	protected override AudioMixerResetPeaksCommand CreateSut(TestCaseData testCase)
	{
		// Create command with default constructor (no state required for reset command)
		var command = new AudioMixerResetPeaksCommand();

		// Set the values that should be sent
		command.All = testCase.Command.All;
		command.Input = testCase.Command.Input;
		command.Master = testCase.Command.Master;
		command.Monitor = testCase.Command.Monitor;
		
		return command;
	}

	[Test]
	public void Constructor_ShouldInitializeWithDefaultValues()
	{
		// Act
		var command = new AudioMixerResetPeaksCommand();

		// Assert
		Assert.That(command.All, Is.False);
		Assert.That(command.Input, Is.EqualTo(0));
		Assert.That(command.Master, Is.False);
		Assert.That(command.Monitor, Is.False);
		Assert.That(command.Flag, Is.EqualTo(0)); // No flags should be set initially
	}

	[Test]
	public void SetAll_ShouldSetCorrectFlag()
	{
		// Arrange
		var command = new AudioMixerResetPeaksCommand();

		// Act
		command.All = true;

		// Assert
		Assert.That(command.All, Is.True);
		Assert.That(command.Flag, Is.EqualTo(1 << 0)); // Bit 0 should be set
	}

	[Test]
	public void SetInput_ShouldSetCorrectFlag()
	{
		// Arrange
		var command = new AudioMixerResetPeaksCommand();

		// Act
		command.Input = 5;

		// Assert
		Assert.That(command.Input, Is.EqualTo(5));
		Assert.That(command.Flag, Is.EqualTo(1 << 1)); // Bit 1 should be set
	}

	[Test]
	public void SetMaster_ShouldSetCorrectFlag()
	{
		// Arrange
		var command = new AudioMixerResetPeaksCommand();

		// Act
		command.Master = true;

		// Assert
		Assert.That(command.Master, Is.True);
		Assert.That(command.Flag, Is.EqualTo(1 << 2)); // Bit 2 should be set
	}

	[Test]
	public void SetMonitor_ShouldSetCorrectFlag()
	{
		// Arrange
		var command = new AudioMixerResetPeaksCommand();

		// Act
		command.Monitor = true;

		// Assert
		Assert.That(command.Monitor, Is.True);
		Assert.That(command.Flag, Is.EqualTo(1 << 3)); // Bit 3 should be set
	}

	[Test]
	public void SetMultipleProperties_ShouldSetCombinedFlags()
	{
		// Arrange
		var command = new AudioMixerResetPeaksCommand();

		// Act
		command.All = true;      // Bit 0
		command.Master = true;   // Bit 2

		// Assert
		Assert.That(command.Flag, Is.EqualTo((1 << 0) | (1 << 2))); // Bits 0 and 2 should be set
	}

	[Test]
	public void Serialize_WithDefaultValues_ShouldProduceCorrectOutput()
	{
		// Arrange
		var command = new AudioMixerResetPeaksCommand();

		// Act
		var result = command.Serialize(ProtocolVersion.V7_2);

		// Assert
		Assert.That(result.Length, Is.EqualTo(8));
		Assert.That(result[0], Is.EqualTo(0)); // Flag = 0
		Assert.That(result[1], Is.EqualTo(0)); // All = false
		Assert.That(result[2], Is.EqualTo(0)); // Input high byte
		Assert.That(result[3], Is.EqualTo(0)); // Input low byte
		Assert.That(result[4], Is.EqualTo(0)); // Master = false
		Assert.That(result[5], Is.EqualTo(0)); // Monitor = false
		Assert.That(result[6], Is.EqualTo(0)); // Padding
		Assert.That(result[7], Is.EqualTo(0)); // Padding
	}

	[Test]
	public void Serialize_WithAllTrue_ShouldProduceCorrectOutput()
	{
		// Arrange
		var command = new AudioMixerResetPeaksCommand();
		command.All = true;

		// Act
		var result = command.Serialize(ProtocolVersion.V7_2);

		// Assert
		Assert.That(result.Length, Is.EqualTo(8));
		Assert.That(result[0], Is.EqualTo(1)); // Flag = 1 (bit 0 set)
		Assert.That(result[1], Is.EqualTo(1)); // All = true
		Assert.That(result[2], Is.EqualTo(0)); // Input high byte
		Assert.That(result[3], Is.EqualTo(0)); // Input low byte
		Assert.That(result[4], Is.EqualTo(0)); // Master = false
		Assert.That(result[5], Is.EqualTo(0)); // Monitor = false
		Assert.That(result[6], Is.EqualTo(0)); // Padding
		Assert.That(result[7], Is.EqualTo(0)); // Padding
	}

	[Test]
	public void Serialize_WithInputSet_ShouldProduceCorrectOutput()
	{
		// Arrange
		var command = new AudioMixerResetPeaksCommand();
		command.Input = 0x0102; // Big-endian test value

		// Act
		var result = command.Serialize(ProtocolVersion.V7_2);

		// Assert
		Assert.That(result.Length, Is.EqualTo(8));
		Assert.That(result[0], Is.EqualTo(2)); // Flag = 2 (bit 1 set)
		Assert.That(result[1], Is.EqualTo(0)); // All = false
		Assert.That(result[2], Is.EqualTo(0x01)); // Input high byte
		Assert.That(result[3], Is.EqualTo(0x02)); // Input low byte
		Assert.That(result[4], Is.EqualTo(0)); // Master = false
		Assert.That(result[5], Is.EqualTo(0)); // Monitor = false
		Assert.That(result[6], Is.EqualTo(0)); // Padding
		Assert.That(result[7], Is.EqualTo(0)); // Padding
	}

	[Test]
	public void Serialize_WithMasterAndMonitor_ShouldProduceCorrectOutput()
	{
		// Arrange
		var command = new AudioMixerResetPeaksCommand();
		command.Master = true;
		command.Monitor = true;

		// Act
		var result = command.Serialize(ProtocolVersion.V7_2);

		// Assert
		Assert.That(result.Length, Is.EqualTo(8));
		Assert.That(result[0], Is.EqualTo(12)); // Flag = 12 (bits 2 and 3 set: 4 + 8 = 12)
		Assert.That(result[1], Is.EqualTo(0)); // All = false
		Assert.That(result[2], Is.EqualTo(0)); // Input high byte
		Assert.That(result[3], Is.EqualTo(0)); // Input low byte
		Assert.That(result[4], Is.EqualTo(1)); // Master = true
		Assert.That(result[5], Is.EqualTo(1)); // Monitor = true
		Assert.That(result[6], Is.EqualTo(0)); // Padding
		Assert.That(result[7], Is.EqualTo(0)); // Padding
	}

	[Test]
	public void Serialize_WithAllFlagsSet_ShouldProduceCorrectOutput()
	{
		// Arrange
		var command = new AudioMixerResetPeaksCommand();
		command.All = true;
		command.Input = 255;
		command.Master = true;
		command.Monitor = true;

		// Act
		var result = command.Serialize(ProtocolVersion.V7_2);

		// Assert
		Assert.That(result.Length, Is.EqualTo(8));
		Assert.That(result[0], Is.EqualTo(15)); // Flag = 15 (bits 0,1,2,3 all set: 1+2+4+8 = 15)
		Assert.That(result[1], Is.EqualTo(1)); // All = true
		Assert.That(result[2], Is.EqualTo(0)); // Input high byte (255 = 0x00FF)
		Assert.That(result[3], Is.EqualTo(255)); // Input low byte
		Assert.That(result[4], Is.EqualTo(1)); // Master = true
		Assert.That(result[5], Is.EqualTo(1)); // Monitor = true
		Assert.That(result[6], Is.EqualTo(0)); // Padding
		Assert.That(result[7], Is.EqualTo(0)); // Padding
	}
}