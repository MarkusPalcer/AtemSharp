using AtemSharp.Commands.DownstreamKey;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.DownstreamKey;

[TestFixture]
public class DownstreamKeyGeneralCommandTests : SerializedCommandTestBase<DownstreamKeyGeneralCommand,
    DownstreamKeyGeneralCommandTests.CommandData>
{
    protected override Range[] GetFloatingPointByteRanges() => [
        (4..6), // Clip
        (6..8) // Gain
    ];

    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public bool PreMultipliedKey { get; set; }
        public double Clip { get; set; }
        public double Gain { get; set; }
        public bool Invert { get; set; }
    }

    protected override DownstreamKeyGeneralCommand CreateSut(TestCaseData testCase)
    {
        // Create state with the required downstream keyer
        var state = CreateDownstreamKeyer(testCase.Command.Index);

        // Create command with the keyer ID
        var command = new DownstreamKeyGeneralCommand(state);

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
    private static DownstreamKeyer CreateDownstreamKeyer(byte keyerId)
    {
        return new DownstreamKeyer
        {
            Id = keyerId,
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
    }

    [Test]
    public void Constructor_WithValidKeyerId_ShouldInitializeCorrectly()
    {
        // Arrange
        const int keyerId = 1;
        var state = CreateDownstreamKeyer(keyerId);

        // Act
        var command = new DownstreamKeyGeneralCommand(state);

        // Assert
        Assert.That(command.DownstreamKeyerId, Is.EqualTo(keyerId));
        Assert.That(command.PreMultiply, Is.False); // Default from state
        Assert.That(command.Clip, Is.EqualTo(0.0)); // Default from state
        Assert.That(command.Gain, Is.EqualTo(0.0)); // Default from state
        Assert.That(command.Invert, Is.False); // Default from state
        Assert.That(command.Flag, Is.EqualTo(0)); // No flags set initially
    }

    [Test]
    public void PreMultiply_WhenSet_ShouldUpdateFlagAndValue()
    {
        // Arrange
        var command = new DownstreamKeyGeneralCommand(CreateDownstreamKeyer(0));

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
        var command = new DownstreamKeyGeneralCommand(CreateDownstreamKeyer(0));

        // Act
        command.Clip = 50.0;

        // Assert
        Assert.That(command.Clip, Is.EqualTo(50.0));
        Assert.That(command.Flag & 2, Is.EqualTo(2)); // Flag bit 1 should be set
    }


    [Test]
    public void Gain_WhenSet_ShouldUpdateFlagAndValue()
    {
        // Arrange
        var command = new DownstreamKeyGeneralCommand(CreateDownstreamKeyer(0));

        // Act
        command.Gain = 75.0;

        // Assert
        Assert.That(command.Gain, Is.EqualTo(75.0));
        Assert.That(command.Flag & 4, Is.EqualTo(4)); // Flag bit 2 should be set
    }

    [Test]
    public void Properties_WhenSetMultipleTimes_ShouldMaintainFlags()
    {
        // Arrange
        var command = new DownstreamKeyGeneralCommand(CreateDownstreamKeyer(0));

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
}
