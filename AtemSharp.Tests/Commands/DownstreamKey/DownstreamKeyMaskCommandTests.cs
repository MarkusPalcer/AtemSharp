using AtemSharp.Commands.DownstreamKey;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.DownstreamKey;

[TestFixture]
public class DownstreamKeyMaskCommandTests : SerializedCommandTestBase<DownstreamKeyMaskCommand,
    DownstreamKeyMaskCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int Index { get; set; }
        public bool MaskEnabled { get; set; }
        public double MaskTop { get; set; }
        public double MaskBottom { get; set; }
        public double MaskLeft { get; set; }
        public double MaskRight { get; set; }
    }

    protected override DownstreamKeyMaskCommand CreateSut(TestCaseData testCase)
    {
        // Create state with the required downstream keyer
        var state = CreateStateWithDownstreamKeyer(testCase.Command.Index);
        
        // Create command with the keyer ID
        var command = new DownstreamKeyMaskCommand(testCase.Command.Index, state);

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
        var command = new DownstreamKeyMaskCommand(keyerId, state);

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
    public void Constructor_WithMissingDownstreamKeyer_ShouldUseDefaults()
    {
        // Arrange
        const int keyerId = 5;
        var state = new AtemState(); // No video state

        // Act
        var command = new DownstreamKeyMaskCommand(keyerId, state);

        // Assert
        Assert.That(command.DownstreamKeyerId, Is.EqualTo(keyerId));
        Assert.That(command.Enabled, Is.False); // Default value
        Assert.That(command.Top, Is.EqualTo(0.0)); // Default value
        Assert.That(command.Bottom, Is.EqualTo(0.0)); // Default value
        Assert.That(command.Left, Is.EqualTo(0.0)); // Default value
        Assert.That(command.Right, Is.EqualTo(0.0)); // Default value
        Assert.That(command.Flag, Is.EqualTo(31)); // All flags should be set due to property assignment (bits 0-4)
    }

    [Test]
    public void Enabled_WhenSet_ShouldUpdateFlagAndValue()
    {
        // Arrange
        var command = new DownstreamKeyMaskCommand(0, CreateStateWithDownstreamKeyer(0));
        
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
        var command = new DownstreamKeyMaskCommand(0, CreateStateWithDownstreamKeyer(0));
        
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
        var command = new DownstreamKeyMaskCommand(0, CreateStateWithDownstreamKeyer(0));
        
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
        var command = new DownstreamKeyMaskCommand(0, CreateStateWithDownstreamKeyer(0));
        
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
        var command = new DownstreamKeyMaskCommand(0, CreateStateWithDownstreamKeyer(0));
        
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
        var command = new DownstreamKeyMaskCommand(0, CreateStateWithDownstreamKeyer(0));
        
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

    [Test]
    public void Serialize_BasicTest_ShouldProduceCorrectBytes()
    {
        // Arrange
        var command = new DownstreamKeyMaskCommand(1, CreateStateWithDownstreamKeyer(1));
        command.Enabled = true;
        command.Top = 0.1;      // Will become 100 when converted (0.1 * 1000)
        command.Bottom = -0.05; // Will become -50 when converted (-0.05 * 1000)
        command.Left = 0.2;     // Will become 200 when converted (0.2 * 1000)
        command.Right = -0.075; // Will become -75 when converted (-0.075 * 1000)

        // Act
        var result = command.Serialize(ProtocolVersion.V8_0);

        // Assert
        Assert.That(result.Length, Is.EqualTo(12));
        Assert.That(result[0], Is.EqualTo(31)); // All flags set (0x1F)
        Assert.That(result[1], Is.EqualTo(1));  // Downstream keyer ID
        Assert.That(result[2], Is.EqualTo(1));  // Enabled = true
        Assert.That(result[3], Is.EqualTo(0));  // Padding
        
        // Check the 16-bit signed values (big-endian) - they should be multiplied by 1000
        var top = (short)((result[4] << 8) | result[5]);
        var bottom = (short)((result[6] << 8) | result[7]);
        var left = (short)((result[8] << 8) | result[9]);
        var right = (short)((result[10] << 8) | result[11]);
        
        Assert.That(top, Is.EqualTo(100));     // 0.1 * 1000
        Assert.That(bottom, Is.EqualTo(-50));  // -0.05 * 1000
        Assert.That(left, Is.EqualTo(200));    // 0.2 * 1000
        Assert.That(right, Is.EqualTo(-75));   // -0.075 * 1000
    }
}