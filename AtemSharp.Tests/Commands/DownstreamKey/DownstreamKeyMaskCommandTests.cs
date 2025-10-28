using AtemSharp.Commands.DownstreamKey;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.DownstreamKey;

[TestFixture]
public class DownstreamKeyMaskCommandTests : SerializedCommandTestBase<DownstreamKeyMaskCommand,
    DownstreamKeyMaskCommandTests.CommandData>
{
    protected override Range[] GetFloatingPointByteRanges() =>
    [
        (4..6), // Top
        (6..8), // Bottom
        (8..10), // Left
        (10..12), // Right
    ];

    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public bool MaskEnabled { get; set; }
        public double MaskTop { get; set; }
        public double MaskBottom { get; set; }
        public double MaskLeft { get; set; }
        public double MaskRight { get; set; }
    }

    protected override DownstreamKeyMaskCommand CreateSut(TestCaseData testCase)
    {
        // Create state with the required downstream keyer
        var state = CreateDownstreamKeyer(testCase.Command.Index);

        // Create command with the keyer ID
        var command = new DownstreamKeyMaskCommand(state);

        // Set the actual values that should be written
        command.Enabled = testCase.Command.MaskEnabled;
        command.Top = testCase.Command.MaskTop;
        command.Bottom = testCase.Command.MaskBottom;
        command.Left = testCase.Command.MaskLeft;
        command.Right = testCase.Command.MaskRight;

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
                Mask = new DownstreamKeyerMask
                {
                    Enabled = false,
                    Top = 0.0,
                    Bottom = 0.0,
                    Left = 0.0,
                    Right = 0.0
                }
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
        var command = new DownstreamKeyMaskCommand(state);

        // Assert
        Assert.That(command.DownstreamKeyerId, Is.EqualTo(keyerId));
        Assert.That(command.Enabled, Is.False); // Default from state
        Assert.That(command.Top, Is.EqualTo(0.0)); // Default from state
        Assert.That(command.Bottom, Is.EqualTo(0.0)); // Default from state
        Assert.That(command.Left, Is.EqualTo(0.0)); // Default from state
        Assert.That(command.Right, Is.EqualTo(0.0)); // Default from state
        Assert.That(command.Flag, Is.EqualTo(0)); // No flags set initially
    }

    [Test]
    public void Enabled_WhenSet_ShouldUpdateFlagAndValue()
    {
        // Arrange
        var command = new DownstreamKeyMaskCommand(CreateDownstreamKeyer(0));

        // Act
        command.Enabled = true;

        // Assert
        Assert.That(command.Enabled, Is.True);
        Assert.That(command.Flag & 1, Is.EqualTo(1)); // Flag bit 0 should be set
    }

    [Test]
    public void Top_WhenSet_ShouldUpdateFlagAndValue()
    {
        // Arrange
        var command = new DownstreamKeyMaskCommand(CreateDownstreamKeyer(0));

        // Act
        command.Top = 100.0;

        // Assert
        Assert.That(command.Top, Is.EqualTo(100.0));
        Assert.That(command.Flag & 2, Is.EqualTo(2)); // Flag bit 1 should be set
    }

    [Test]
    public void Bottom_WhenSet_ShouldUpdateFlagAndValue()
    {
        // Arrange
        var command = new DownstreamKeyMaskCommand(CreateDownstreamKeyer(0));
        // Act
        command.Bottom = 200.0;

        // Assert
        Assert.That(command.Bottom, Is.EqualTo(200.0));
        Assert.That(command.Flag & 4, Is.EqualTo(4)); // Flag bit 2 should be set
    }

    [Test]
    public void Left_WhenSet_ShouldUpdateFlagAndValue()
    {
        // Arrange
        var command = new DownstreamKeyMaskCommand(CreateDownstreamKeyer(0));

        // Act
        command.Left = 150.0;

        // Assert
        Assert.That(command.Left, Is.EqualTo(150.0));
        Assert.That(command.Flag & 8, Is.EqualTo(8)); // Flag bit 3 should be set
    }

    [Test]
    public void Right_WhenSet_ShouldUpdateFlagAndValue()
    {
        // Arrange
        var command = new DownstreamKeyMaskCommand(CreateDownstreamKeyer(0));

        // Act
        command.Right = 300.0;

        // Assert
        Assert.That(command.Right, Is.EqualTo(300.0));
        Assert.That(command.Flag & 16, Is.EqualTo(16)); // Flag bit 4 should be set
    }

    [Test]
    public void Multiple_Properties_WhenSet_ShouldUpdateMultipleFlags()
    {
        // Arrange
        var command = new DownstreamKeyMaskCommand(CreateDownstreamKeyer(0));

        // Act
        command.Enabled = true;  // Flag bit 0
        command.Top = 50.0;      // Flag bit 1
        command.Left = 75.0;     // Flag bit 3

        // Assert
        Assert.That(command.Enabled, Is.True);
        Assert.That(command.Top, Is.EqualTo(50.0));
        Assert.That(command.Left, Is.EqualTo(75.0));
        Assert.That(command.Flag, Is.EqualTo(11)); // Bits 0, 1, and 3 should be set (1 + 2 + 8 = 11)
    }
}
