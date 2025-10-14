using AtemSharp.Commands.DownstreamKey;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.DownstreamKey;

[TestFixture]
public class DownstreamKeyAutoCommandTests : SerializedCommandTestBase<DownstreamKeyAutoCommand,
    DownstreamKeyAutoCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int Index { get; set; }
        public bool IsTowardsOnAir { get; set; }
    }

    protected override DownstreamKeyAutoCommand CreateSut(TestCaseData testCase)
    {
        // Create state with the required downstream keyer
        var state = CreateStateWithDownstreamKeyer(testCase.Command.Index);

        // Create command with the keyer ID
        var command = new DownstreamKeyAutoCommand(testCase.Command.Index, state);

        // Set the actual values that should be written
        command.IsTowardsOnAir = testCase.Command.IsTowardsOnAir;

        return command;
    }

    /// <summary>
    /// Creates an AtemState with a valid downstream keyer at the specified index
    /// </summary>
    private static AtemState CreateStateWithDownstreamKeyer(int keyerId)
    {
        var downstreamKeyers = AtemStateUtil.CreateArray<DownstreamKeyer>(keyerId + 1);
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
        var command = new DownstreamKeyAutoCommand(keyerId, state);

        // Assert
        Assert.That(command.DownstreamKeyerId, Is.EqualTo(keyerId));
        Assert.That(command.IsTowardsOnAir, Is.False); // Default from state
        Assert.That(command.Flag, Is.EqualTo(0)); // No flags set initially
    }

    [Test]
    public void Constructor_WithMissingDownstreamKeyer_ShouldThrow()
    {
        // Arrange
        const int keyerId = 5;
        var state = new AtemState(); // No video state

        // Act
        Assert.Throws<IndexOutOfRangeException>(() => new DownstreamKeyAutoCommand(keyerId, state));
    }

    [Test]
    public void IsTowardsOnAir_WhenSet_ShouldUpdateFlagAndValue()
    {
        // Arrange
        var command = new DownstreamKeyAutoCommand(0, CreateStateWithDownstreamKeyer(0));

        // Act
        command.IsTowardsOnAir = true;

        // Assert
        Assert.That(command.IsTowardsOnAir, Is.True);
        Assert.That(command.Flag, Is.EqualTo(1)); // Flag bit 0 should be set
    }

    [Test]
    public void IsTowardsOnAir_WhenSetMultipleTimes_ShouldMaintainFlag()
    {
        // Arrange
        var command = new DownstreamKeyAutoCommand(0, CreateStateWithDownstreamKeyer(0));

        // Act
        command.IsTowardsOnAir = true;
        command.IsTowardsOnAir = false;

        // Assert
        Assert.That(command.IsTowardsOnAir, Is.False);
        Assert.That(command.Flag, Is.EqualTo(1)); // Flag should remain set
    }

    [TestCase(0, false)]
    [TestCase(0, true)]
    [TestCase(1, false)]
    [TestCase(1, true)]
    public void Serialize_WithDifferentValues_ShouldProduceCorrectOutput(int keyerId, bool isTowardsOnAir)
    {
        // Arrange
        var command = new DownstreamKeyAutoCommand(keyerId, CreateStateWithDownstreamKeyer(keyerId));
        command.IsTowardsOnAir = isTowardsOnAir;

        // Act
        var result = command.Serialize(ProtocolVersion.V8_1_1);

        // Assert
        Assert.That(result.Length, Is.EqualTo(4));
        Assert.That(result[0], Is.EqualTo(1)); // Flag should be set
        Assert.That(result[1], Is.EqualTo(keyerId)); // Keyer ID
        Assert.That(result[2], Is.EqualTo(isTowardsOnAir ? 1 : 0)); // IsTowardsOnAir
        Assert.That(result[3], Is.EqualTo(0)); // Padding
    }

    [Test]
    public void Serialize_WithLegacyProtocol_ShouldUseLegacyFormat()
    {
        // Arrange
        var command = new DownstreamKeyAutoCommand(1, CreateStateWithDownstreamKeyer(1));
        command.IsTowardsOnAir = true;

        // Act
        var result = command.Serialize(ProtocolVersion.V7_5_2);

        // Assert
        Assert.That(result.Length, Is.EqualTo(4));
        Assert.That(result[0], Is.EqualTo(1)); // Keyer ID (no flag in legacy format)
        Assert.That(result[1], Is.EqualTo(0)); // Padding
        Assert.That(result[2], Is.EqualTo(0)); // Padding
        Assert.That(result[3], Is.EqualTo(0)); // Padding
    }
}
