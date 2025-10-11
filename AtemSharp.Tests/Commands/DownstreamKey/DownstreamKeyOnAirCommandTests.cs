using AtemSharp.Commands.DownstreamKey;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.DownstreamKey;

[TestFixture]
public class DownstreamKeyOnAirCommandTests : SerializedCommandTestBase<DownstreamKeyOnAirCommand,
    DownstreamKeyOnAirCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int Index { get; set; }
        public bool OnAir { get; set; }
    }

    protected override DownstreamKeyOnAirCommand CreateSut(TestCaseData testCase)
    {
        // Create state with the required downstream keyer
        var state = CreateStateWithDownstreamKeyer(testCase.Command.Index);
        
        // Create command with the keyer ID
        var command = new DownstreamKeyOnAirCommand(testCase.Command.Index, state);

        // Set the actual values that should be written
        command.OnAir = testCase.Command.OnAir;
        
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
            IsTowardsOnAir = false
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
        var command = new DownstreamKeyOnAirCommand(keyerId, state);

        // Assert
        Assert.That(command.DownstreamKeyerId, Is.EqualTo(keyerId));
        Assert.That(command.OnAir, Is.False); // Default from state
        Assert.That(command.Flag, Is.EqualTo(0)); // No flags set initially
    }

    [Test]
    public void Constructor_WithMissingDownstreamKeyer_ShouldUseDefaults()
    {
        // Arrange
        const int keyerId = 5;
        var state = new AtemState(); // No video state

        // Act
        var command = new DownstreamKeyOnAirCommand(keyerId, state);

        // Assert
        Assert.That(command.DownstreamKeyerId, Is.EqualTo(keyerId));
        Assert.That(command.OnAir, Is.False); // Default value
        Assert.That(command.Flag, Is.EqualTo(1)); // Flag should be set due to property assignment
    }

    [Test]
    public void OnAir_WhenSet_ShouldUpdateFlagAndValue()
    {
        // Arrange
        var command = new DownstreamKeyOnAirCommand(0, CreateStateWithDownstreamKeyer(0));
        
        // Act
        command.OnAir = true;

        // Assert
        Assert.That(command.OnAir, Is.True);
        Assert.That(command.Flag, Is.EqualTo(1)); // Flag bit 0 should be set
    }

    [Test]
    public void OnAir_WhenSetMultipleTimes_ShouldMaintainFlag()
    {
        // Arrange
        var command = new DownstreamKeyOnAirCommand(0, CreateStateWithDownstreamKeyer(0));
        
        // Act
        command.OnAir = true;
        command.OnAir = false;

        // Assert
        Assert.That(command.OnAir, Is.False);
        Assert.That(command.Flag, Is.EqualTo(1)); // Flag should remain set
    }

    [TestCase(0, false)]
    [TestCase(0, true)]
    [TestCase(1, false)]
    [TestCase(1, true)]
    [TestCase(2, false)]
    [TestCase(2, true)]
    public void Serialize_WithDifferentValues_ShouldProduceCorrectOutput(int keyerId, bool onAir)
    {
        // Arrange
        var command = new DownstreamKeyOnAirCommand(keyerId, CreateStateWithDownstreamKeyer(keyerId));
        command.OnAir = onAir;

        // Act
        var result = command.Serialize(ProtocolVersion.V8_1_1);

        // Assert
        Assert.That(result.Length, Is.EqualTo(4));
        Assert.That(result[0], Is.EqualTo(keyerId)); // Keyer ID at byte 0
        Assert.That(result[1], Is.EqualTo(onAir ? 1 : 0)); // OnAir at byte 1
        Assert.That(result[2], Is.EqualTo(0)); // Padding
        Assert.That(result[3], Is.EqualTo(0)); // Padding
    }

    [Test]
    public void Serialize_WithNoFlagSet_ShouldStillIncludeValues()
    {
        // Arrange
        var state = CreateStateWithDownstreamKeyer(0);
        state.Video.DownstreamKeyers[0].OnAir = true; // Set initial state
        
        var command = new DownstreamKeyOnAirCommand(0, state);
        // Don't set any properties - use state defaults

        // Act
        var result = command.Serialize(ProtocolVersion.V8_1_1);

        // Assert
        Assert.That(result.Length, Is.EqualTo(4));
        Assert.That(result[0], Is.EqualTo(0)); // Keyer ID
        Assert.That(result[1], Is.EqualTo(1)); // OnAir from state
        Assert.That(result[2], Is.EqualTo(0)); // Padding
        Assert.That(result[3], Is.EqualTo(0)); // Padding
    }

    [Test]
    public void Serialize_WithLargeKeyerId_ShouldHandleCorrectly()
    {
        // Arrange
        const int keyerId = 255; // Max value for byte
        var command = new DownstreamKeyOnAirCommand(keyerId, CreateStateWithDownstreamKeyer(keyerId));
        command.OnAir = true;

        // Act
        var result = command.Serialize(ProtocolVersion.V8_1_1);

        // Assert
        Assert.That(result.Length, Is.EqualTo(4));
        Assert.That(result[0], Is.EqualTo(255)); // Keyer ID
        Assert.That(result[1], Is.EqualTo(1)); // OnAir
    }
}