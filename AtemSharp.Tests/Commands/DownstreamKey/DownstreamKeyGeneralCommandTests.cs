using AtemSharp.Commands.DownstreamKey;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.DownstreamKey;

[TestFixture]
public class DownstreamKeyGeneralCommandTests : SerializedCommandTestBase<DownstreamKeyGeneralCommand,
    DownstreamKeyGeneralCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int Index { get; set; }
        public bool PreMultipliedKey { get; set; }
        public double Clip { get; set; }
        public double Gain { get; set; }
        public bool Invert { get; set; }
    }

    protected override DownstreamKeyGeneralCommand CreateSut(TestCaseData testCase)
    {
        // Create state with the required downstream keyer
        var state = CreateStateWithDownstreamKeyer(testCase.Command.Index);
        
        // Create command with the keyer ID
        var command = new DownstreamKeyGeneralCommand(testCase.Command.Index, state);

        // Set the actual values that should be written
        command.PreMultiply = testCase.Command.PreMultipliedKey;
        command.Clip = testCase.Command.Clip;
        command.Gain = testCase.Command.Gain;
        command.Invert = testCase.Command.Invert;
        
        return command;
    }

    /// <summary>
    /// Creates an AtemState with a valid downstream keyer at the specified index
    /// </summary>
    private static AtemState CreateStateWithDownstreamKeyer(int keyerId)
    {
        Dictionary<int, DownstreamKeyer> downstreamKeyers = new Dictionary<int, DownstreamKeyer>();
        downstreamKeyers[keyerId] = new DownstreamKeyer
        {
            InTransition = false,
            RemainingFrames = 0,
            IsAuto = false,
            OnAir = false,
            IsTowardsOnAir = false,
            Properties = new DownstreamKeyerProperties
            {
                PreMultiply = false,
                Clip = 0.0,
                Gain = 0.0,
                Invert = false,
                Tie = false,
                Rate = 25,
                Mask = new DownstreamKeyerMask()
            }
        };

        var state = new AtemState
        {
            Video = new VideoState
            {
                DownstreamKeyers = downstreamKeyers
            }
        };
        return state;
    }

    [Test]
    public void Constructor_WithValidKeyerId_ShouldInitializeCorrectly()
    {
        // Arrange
        const int keyerId = 1;
        var state = CreateStateWithDownstreamKeyer(keyerId);

        // Act
        var command = new DownstreamKeyGeneralCommand(keyerId, state);

        // Assert
        Assert.That(command.DownstreamKeyerId, Is.EqualTo(keyerId));
        Assert.That(command.PreMultiply, Is.False); // Default from state
        Assert.That(command.Clip, Is.EqualTo(0.0)); // Default from state
        Assert.That(command.Gain, Is.EqualTo(0.0)); // Default from state
        Assert.That(command.Invert, Is.False); // Default from state
        Assert.That(command.Flag, Is.EqualTo(0)); // No flags set initially
    }

    [Test]
    public void Constructor_WithMissingDownstreamKeyer_ShouldUseDefaults()
    {
        // Arrange
        const int keyerId = 5;
        var state = new AtemState(); // No video state

        // Act
        var command = new DownstreamKeyGeneralCommand(keyerId, state);

        // Assert
        Assert.That(command.DownstreamKeyerId, Is.EqualTo(keyerId));
        Assert.That(command.PreMultiply, Is.False); // Default value
        Assert.That(command.Clip, Is.EqualTo(0.0)); // Default value
        Assert.That(command.Gain, Is.EqualTo(0.0)); // Default value
        Assert.That(command.Invert, Is.False); // Default value
        Assert.That(command.Flag, Is.EqualTo(15)); // All flags should be set due to property assignment
    }

    [Test]
    public void PreMultiply_WhenSet_ShouldUpdateFlagAndValue()
    {
        // Arrange
        var command = new DownstreamKeyGeneralCommand(0, CreateStateWithDownstreamKeyer(0));
        
        // Act
        command.PreMultiply = true;

        // Assert
        Assert.That(command.PreMultiply, Is.True);
        Assert.That(command.Flag & 1, Is.EqualTo(1)); // Flag bit 0 should be set
    }

    [Test]
    public void Clip_WhenSet_ShouldUpdateFlagAndValue()
    {
        // Arrange
        var command = new DownstreamKeyGeneralCommand(0, CreateStateWithDownstreamKeyer(0));
        
        // Act
        command.Clip = 50.0;

        // Assert
        Assert.That(command.Clip, Is.EqualTo(50.0));
        Assert.That(command.Flag & 2, Is.EqualTo(2)); // Flag bit 1 should be set
    }

    [Test]
    public void Clip_WithInvalidValue_ShouldThrowException()
    {
        // Arrange
        var command = new DownstreamKeyGeneralCommand(0, CreateStateWithDownstreamKeyer(0));
        
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => command.Clip = -1.0);
        Assert.Throws<ArgumentOutOfRangeException>(() => command.Clip = 101.0);
    }

    [Test]
    public void Gain_WhenSet_ShouldUpdateFlagAndValue()
    {
        // Arrange
        var command = new DownstreamKeyGeneralCommand(0, CreateStateWithDownstreamKeyer(0));
        
        // Act
        command.Gain = 75.0;

        // Assert
        Assert.That(command.Gain, Is.EqualTo(75.0));
        Assert.That(command.Flag & 4, Is.EqualTo(4)); // Flag bit 2 should be set
    }

    [Test]
    public void Gain_WithInvalidValue_ShouldThrowException()
    {
        // Arrange
        var command = new DownstreamKeyGeneralCommand(0, CreateStateWithDownstreamKeyer(0));
        
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => command.Gain = -1.0);
        Assert.Throws<ArgumentOutOfRangeException>(() => command.Gain = 101.0);
    }

    [Test]
    public void Invert_WhenSet_ShouldUpdateFlagAndValue()
    {
        // Arrange
        var command = new DownstreamKeyGeneralCommand(0, CreateStateWithDownstreamKeyer(0));
        
        // Act
        command.Invert = true;

        // Assert
        Assert.That(command.Invert, Is.True);
        Assert.That(command.Flag & 8, Is.EqualTo(8)); // Flag bit 3 should be set
    }

    [Test]
    public void Properties_WhenSetMultipleTimes_ShouldMaintainFlags()
    {
        // Arrange
        var command = new DownstreamKeyGeneralCommand(0, CreateStateWithDownstreamKeyer(0));
        
        // Act
        command.PreMultiply = true;
        command.Clip = 25.0;
        command.PreMultiply = false; // Change again
        command.Gain = 50.0;

        // Assert
        Assert.That(command.PreMultiply, Is.False);
        Assert.That(command.Clip, Is.EqualTo(25.0));
        Assert.That(command.Gain, Is.EqualTo(50.0));
        Assert.That(command.Flag, Is.EqualTo(7)); // Bits 0, 1, 2 should be set (1 + 2 + 4 = 7)
    }

    [Test]
    public void Serialize_WithSpecificFlags_ShouldProduceCorrectOutput()
    {
        // Arrange
        var command = new DownstreamKeyGeneralCommand(1, CreateStateWithDownstreamKeyer(1));
        command.PreMultiply = true;
        command.Clip = 25.0;
        command.Gain = 50.0;
        command.Invert = false;
        
        // Override flags to test specific mask combinations
        command.Flag = 7; // Only preMultiply + clip + gain flags

        // Act
        var result = command.Serialize(ProtocolVersion.V8_1_1);

        // Assert
        Assert.That(result.Length, Is.EqualTo(12));
        Assert.That(result[0], Is.EqualTo(7)); // Flag should match what we set
        Assert.That(result[1], Is.EqualTo(1)); // Keyer ID
        Assert.That(result[2], Is.EqualTo(1)); // PreMultiply = true
        Assert.That(result[3], Is.EqualTo(0)); // Padding
        Assert.That(result[4], Is.EqualTo(0)); // Clip high byte (250 = 0x00FA)
        Assert.That(result[5], Is.EqualTo(250)); // Clip low byte
        Assert.That(result[6], Is.EqualTo(1)); // Gain high byte (500 = 0x01F4)
        Assert.That(result[7], Is.EqualTo(244)); // Gain low byte
        Assert.That(result[8], Is.EqualTo(0)); // Invert = false
        Assert.That(result[9], Is.EqualTo(0)); // Padding
        Assert.That(result[10], Is.EqualTo(0)); // Padding
        Assert.That(result[11], Is.EqualTo(0)); // Padding
    }
}