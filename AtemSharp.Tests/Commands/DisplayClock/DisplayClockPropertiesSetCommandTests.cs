using System.Drawing;
using AtemSharp.Commands.DisplayClock;
using AtemSharp.State.DisplayClock;

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

    [Test]
    public void Constructor_ShouldNotSetFlags()
    {
        // Arrange
        var state = new AtemSharp.State.DisplayClock.DisplayClock();

        // Act
        var command = new DisplayClockPropertiesSetCommand(state);

        // Assert
        Assert.That(command.Flag, Is.EqualTo(0)); // No flags should be set initially
    }

    [Test]
    public void PropertySetters_ShouldSetCorrectFlags()
    {
        // Arrange
        var state = new AtemSharp.State.DisplayClock.DisplayClock();
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

    protected override DisplayClockPropertiesSetCommand CreateSut(TestCaseData testCase)
    {
        return new DisplayClockPropertiesSetCommand(new AtemSharp.State.DisplayClock.DisplayClock
        {
            Enabled = testCase.Command.Enabled,
            Size = testCase.Command.Size,
            Opacity = testCase.Command.Opacity,
            Location = new PointF((float)testCase.Command.PositionX,
                                  (float)testCase.Command.PositionY),
            AutoHide = testCase.Command.AutoHide,
            StartFrom = new DisplayClockTime
            {
                Hours = testCase.Command.StartFrom.Hour,
                Minutes = testCase.Command.StartFrom.Minute,
                Seconds = testCase.Command.StartFrom.Second,
                Frames = testCase.Command.StartFrom.Frame
            },
            ClockMode = testCase.Command.ClockMode
        })
        {
            StartFromFrames = testCase.Command.StartFromFrames
        };
    }

    [Test]
    public void SettingLocation_ShouldSetPositionXAndPositionY()
    {
        var state = new AtemSharp.State.DisplayClock.DisplayClock();
        var sut =  new DisplayClockPropertiesSetCommand(state)
        {
            Location = new PointF((float)12.3, (float)45.6)
        };

        Assert.Multiple(() =>
        {
            Assert.That(sut.PositionX, Is.EqualTo(12.3).Within(0.01));
            Assert.That(sut.PositionY, Is.EqualTo(45.6).Within(0.01));
        });
    }

    [Test]
    public void BettingLocation_ShouldGetPositionXAndPositionY()
    {
        var state = new AtemSharp.State.DisplayClock.DisplayClock();
        var sut =  new DisplayClockPropertiesSetCommand(state)
        {
            PositionX = 12.3,
            PositionY = 45.6
        };

        Assert.Multiple(() =>
        {
            Assert.That(sut.Location.X, Is.EqualTo(12.3).Within(0.01));
            Assert.That(sut.Location.Y, Is.EqualTo(45.6).Within(0.01));
        });
    }
}
