using AtemSharp.Commands.DisplayClock;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.DisplayClock;

[TestFixture]
public class DisplayClockPropertiesSetCommandTests : SerializedCommandTestBase<DisplayClockPropertiesSetCommand,
    DisplayClockPropertiesSetCommandTests.CommandData>
{
    protected override Range[] GetFloatingPointByteRanges()
    {
        return
        [
            8..10,  // bytes 8-9 for PositionX 
            10..12  // bytes 10-11 for PositionY
        ];
    }

    public class CommandData : CommandDataBase
    {
        public bool Enabled { get; set; }
        public byte Size { get; set; }
        public byte Opacity { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public bool AutoHide { get; set; }
        public TimeData StartFrom { get; set; } = new();
        public uint StartFromFrames { get; set; }
        public DisplayClockClockMode ClockMode { get; set; }
    }

    public class TimeData
    {
        public byte Hour { get; set; }
        public byte Minute { get; set; }
        public byte Second { get; set; }
        public byte Frame { get; set; }
    }

    /// <summary>
    /// Creates an AtemState with a valid display clock containing default values
    /// </summary>
    private static AtemState CreateStateWithDisplayClock()
    {
        return new AtemState
        {
            DisplayClock = new AtemSharp.State.DisplayClock
            {
                Enabled = false,
                Size = 0,
                Opacity = 255,
                PositionX = 0.0,
                PositionY = 0.0,
                AutoHide = false,
                StartFrom = new DisplayClockTime(),
                ClockMode = DisplayClockClockMode.Countdown,
                ClockState = DisplayClockClockState.Stopped
            }
        };
    }

    [Test]
    public void Constructor_WithValidState_ShouldInitializeCorrectly()
    {
        // Arrange
        var state = CreateStateWithDisplayClock();

        // Act
        var command = new DisplayClockPropertiesSetCommand(state);

        // Assert
        Assert.That(command.Enabled, Is.EqualTo(false));
        Assert.That(command.Size, Is.EqualTo(0));
        Assert.That(command.Opacity, Is.EqualTo(255));
        Assert.That(command.PositionX, Is.EqualTo(0.0));
        Assert.That(command.PositionY, Is.EqualTo(0.0));
        Assert.That(command.AutoHide, Is.EqualTo(false));
        Assert.That(command.StartFrom, Is.Not.Null);
        Assert.That(command.StartFromFrames, Is.EqualTo(0));
        Assert.That(command.ClockMode, Is.EqualTo(DisplayClockClockMode.Countdown));
        Assert.That(command.Flag, Is.EqualTo(0)); // No flags should be set initially
    }

    [Test]
    public void Constructor_WithNullDisplayClock_ShouldSetDefaults()
    {
        // Arrange
        var state = new AtemState { DisplayClock = null };

        // Act
        var command = new DisplayClockPropertiesSetCommand(state);

        // Assert - all flags should be set when no existing state
        Assert.That(command.Flag, Is.EqualTo((1 << 9) - 1)); // All 9 flags should be set (bits 0-8)
    }

    [Test]
    public void PropertySetters_ShouldSetCorrectFlags()
    {
        // Arrange
        var state = CreateStateWithDisplayClock();
        var command = new DisplayClockPropertiesSetCommand(state);

        // Act & Assert
        command.Enabled = true;
        Assert.That(command.Flag & (1 << 0), Is.Not.EqualTo(0));

        command.Size = 42;
        Assert.That(command.Flag & (1 << 1), Is.Not.EqualTo(0));

        command.Opacity = 128;
        Assert.That(command.Flag & (1 << 2), Is.Not.EqualTo(0));

        command.PositionX = 0.5;
        Assert.That(command.Flag & (1 << 3), Is.Not.EqualTo(0));

        command.PositionY = -0.3;
        Assert.That(command.Flag & (1 << 4), Is.Not.EqualTo(0));

        command.AutoHide = true;
        Assert.That(command.Flag & (1 << 5), Is.Not.EqualTo(0));

        command.StartFrom = new DisplayClockTime { Hours = 1 };
        Assert.That(command.Flag & (1 << 6), Is.Not.EqualTo(0));

        command.StartFromFrames = 1000;
        Assert.That(command.Flag & (1 << 7), Is.Not.EqualTo(0));

        command.ClockMode = DisplayClockClockMode.Countup;
        Assert.That(command.Flag & (1 << 8), Is.Not.EqualTo(0));
    }

    [TestCase(-1.0, 0.5)]
    [TestCase(0.0, 0.0)]
    [TestCase(0.999, -0.999)]
    public void PositionProperties_ShouldAcceptValidValues(double x, double y)
    {
        // Arrange
        var state = CreateStateWithDisplayClock();
        var command = new DisplayClockPropertiesSetCommand(state);

        // Act & Assert
        Assert.DoesNotThrow(() => command.PositionX = x);
        Assert.DoesNotThrow(() => command.PositionY = y);
        Assert.That(command.PositionX, Is.EqualTo(x));
        Assert.That(command.PositionY, Is.EqualTo(y));
    }

    protected override DisplayClockPropertiesSetCommand CreateSut(TestCaseData testCase)
    {
        // Create a new state with default display clock values
        var state = new AtemState();
        state.DisplayClock = new AtemSharp.State.DisplayClock
        {
            Enabled = false,
            Size = 0,
            Opacity = 255,
            PositionX = 0.0,
            PositionY = 0.0,
            AutoHide = false,
            StartFrom = new DisplayClockTime(),
            ClockMode = DisplayClockClockMode.Countdown
        };

        var command = new DisplayClockPropertiesSetCommand(state);
        
        // Reset all flags to 0 - the test framework will set the correct flag later
        command.Flag = 0;

        // Set ALL properties from the test data (not just those matching the mask)
        // The mask is only used to control which flags are set, not which properties have values
        var data = testCase.Command;
        
        command.Enabled = data.Enabled;
        command.Size = data.Size;
        command.Opacity = data.Opacity;
        command.PositionX = data.PositionX;
        command.PositionY = data.PositionY;
        command.AutoHide = data.AutoHide;
        command.StartFrom = new DisplayClockTime 
        { 
            Hours = data.StartFrom.Hour, 
            Minutes = data.StartFrom.Minute, 
            Seconds = data.StartFrom.Second, 
            Frames = data.StartFrom.Frame 
        };
        command.StartFromFrames = data.StartFromFrames;
        command.ClockMode = data.ClockMode;

        // Reset flag again after setting properties (since property setters automatically set flags)
        command.Flag = 0;
        
        return command;
    }
}