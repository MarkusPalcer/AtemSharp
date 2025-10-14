using AtemSharp.Commands;
using AtemSharp.Commands.DownstreamKey;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.DownstreamKey;

[TestFixture]
public class DownstreamKeyFillSourceCommandTests : SerializedCommandTestBase<DownstreamKeyFillSourceCommand,
    DownstreamKeyFillSourceCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int Index { get; set; }
        public int FillSource { get; set; }
    }

    protected override DownstreamKeyFillSourceCommand CreateSut(TestCaseData testCase)
    {
        // Create state with the required downstream keyer
        var state = CreateStateWithDownstreamKeyer(testCase.Command.Index, testCase.Command.FillSource);

        // Create command with the keyer ID
        var command = new DownstreamKeyFillSourceCommand(testCase.Command.Index, state)
        {
	        // Set the actual input value that should be written
	        Input = testCase.Command.FillSource
        };

        return command;
    }

    /// <summary>
    /// Creates an AtemState with a valid downstream keyer at the specified index
    /// </summary>
    private static AtemState CreateStateWithDownstreamKeyer(int keyerId, int fillSource = 0)
    {
        var downstreamKeyers = AtemStateUtil.CreateArray<DownstreamKeyer>(keyerId+1);
        downstreamKeyers[keyerId] = new()
	        {
		        InTransition = false,
		        RemainingFrames = 0,
		        IsAuto = false,
		        OnAir = false,
		        IsTowardsOnAir = false,
		        Sources = new DownstreamKeyerSources
		        {
			        FillSource = fillSource,
			        CutSource = 1000
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
        const int expectedFillSource = 42;
        var state = CreateStateWithDownstreamKeyer(keyerId, expectedFillSource);

        // Act
        var command = new DownstreamKeyFillSourceCommand(keyerId, state);

        // Assert
        Assert.That(command.DownstreamKeyerId, Is.EqualTo(keyerId));
        Assert.That(command.Input, Is.EqualTo(expectedFillSource)); // Should get value from state
        Assert.That(command.Flag, Is.EqualTo(0)); // No flags set initially
    }

    [Test]
    public void Constructor_WithMissingDownstreamKeyer_ShouldUseDefaults()
    {
        // Arrange
        const int keyerId = 5;
        var state = new AtemState(); // No video state

        // Act
        Assert.Throws<IndexOutOfRangeException>(() =>new DownstreamKeyFillSourceCommand(keyerId, state));
    }

    [Test]
    public void Input_WhenSet_ShouldUpdateFlagAndValue()
    {
        // Arrange
        var command = new DownstreamKeyFillSourceCommand(0, CreateStateWithDownstreamKeyer(0))
        {
	        // Act
	        Input = 1234
        };

        // Assert
        Assert.That(command.Input, Is.EqualTo(1234));
        Assert.That(command.Flag, Is.EqualTo(1)); // Flag bit 0 should be set
    }

    [Test]
    public void Input_WhenSetMultipleTimes_ShouldMaintainFlag()
    {
        // Arrange
        var command = new DownstreamKeyFillSourceCommand(0, CreateStateWithDownstreamKeyer(0))
        {
	        // Act
	        Input = 100
        };

        command.Input = 200;

        // Assert
        Assert.That(command.Input, Is.EqualTo(200));
        Assert.That(command.Flag, Is.EqualTo(1)); // Flag should remain set
    }

    [TestCase(0, 0)]
    [TestCase(0, 1)]
    [TestCase(0, 1000)]
    [TestCase(0, 65535)]
    [TestCase(1, 42)]
    [TestCase(1, 1234)]
    public void Serialize_WithDifferentValues_ShouldProduceCorrectOutput(int keyerId, int input)
    {
        // Arrange
        var command = new DownstreamKeyFillSourceCommand(keyerId, CreateStateWithDownstreamKeyer(keyerId))
        {
	        Input = input
        };

        // Act
        var result = command.Serialize(ProtocolVersion.V8_1_1);

        // Assert
        Assert.That(result.Length, Is.EqualTo(4));
        Assert.That(result[0], Is.EqualTo(keyerId)); // Keyer ID
        Assert.That(result[1], Is.EqualTo(0)); // Padding

        // Input as 16-bit big-endian (bytes 2-3)
        var expectedInputBytes = BitConverter.GetBytes((ushort)input);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(expectedInputBytes);
        }
        Assert.That(result[2], Is.EqualTo(expectedInputBytes[0])); // High byte
        Assert.That(result[3], Is.EqualTo(expectedInputBytes[1])); // Low byte
    }

    [Test]
    public void Serialize_WithMaxValues_ShouldHandleCorrectly()
    {
        // Arrange
        const int maxKeyerId = 255;
        const int maxInput = 65535;
        var command = new DownstreamKeyFillSourceCommand(maxKeyerId, CreateStateWithDownstreamKeyer(maxKeyerId))
        {
	        Input = maxInput
        };

        // Act
        var result = command.Serialize(ProtocolVersion.V8_1_1);

        // Assert
        Assert.That(result.Length, Is.EqualTo(4));
        Assert.That(result[0], Is.EqualTo(maxKeyerId)); // Keyer ID
        Assert.That(result[1], Is.EqualTo(0)); // Padding
        Assert.That(result[2], Is.EqualTo(0xFF)); // High byte of 65535
        Assert.That(result[3], Is.EqualTo(0xFF)); // Low byte of 65535
    }

    [Test]
    public void Serialize_WithLegacyProtocol_ShouldProduceSameOutput()
    {
        // Arrange
        const int keyerId = 1;
        const int input = 500;
        var command = new DownstreamKeyFillSourceCommand(keyerId, CreateStateWithDownstreamKeyer(keyerId))
        {
	        Input = input
        };

        // Act
        var modernResult = command.Serialize(ProtocolVersion.V8_1_1);
        var legacyResult = command.Serialize(ProtocolVersion.V7_5_2);

        // Assert - this command format doesn't change between protocol versions
        Assert.That(legacyResult, Is.EqualTo(modernResult));
    }

    [TestCase(0, 1000)]
    [TestCase(1, 2000)]
    [TestCase(0, 3000)]
    public void RoundTrip_WithDifferentInputValues_ShouldMaintainConsistency(int keyerId, int input)
    {
        // Arrange
        var command = new DownstreamKeyFillSourceCommand(keyerId, CreateStateWithDownstreamKeyer(keyerId))
        {
	        Input = input
        };

        // Act
        var serialized = command.Serialize(ProtocolVersion.V8_1_1);

        // Verify we can extract the input back from the serialized data
        var extractedKeyerId = serialized[0];
        var extractedInput = (serialized[2] << 8) | serialized[3];

        // Assert
        Assert.That(extractedKeyerId, Is.EqualTo(keyerId));
        Assert.That(extractedInput, Is.EqualTo(input));
    }

    [Test]
    public void CommandAttribute_ShouldHaveCorrectRawName()
    {
        // Arrange & Act
        var commandType = typeof(DownstreamKeyFillSourceCommand);
        var attribute = commandType.GetCustomAttributes(typeof(CommandAttribute), false)
            .Cast<CommandAttribute>()
            .FirstOrDefault();

        // Assert
        Assert.That(attribute, Is.Not.Null, "Command should have CommandAttribute");
        Assert.That(attribute!.RawName, Is.EqualTo("CDsF"), "Command should have correct raw name");
    }

    [Test]
    public void Constructor_WithKeyerIndexOutOfBounds_ShouldUseDefaults()
    {
        // Arrange
        const int keyerId = 10; // Higher than available keyers
        var downstreamKeyers = AtemStateUtil.CreateArray<DownstreamKeyer>(2);
	        downstreamKeyers[0] = new()
	        {
		        Sources = new DownstreamKeyerSources { FillSource = 100, CutSource = 200 }
        }; // Only 2 keyers available

        var state = new AtemState
        {
            Video = new VideoState
            {
                DownstreamKeyers = downstreamKeyers
            }
        };

        // Act
        Assert.Throws<IndexOutOfRangeException>(() => new DownstreamKeyFillSourceCommand(keyerId, state));
    }
}
