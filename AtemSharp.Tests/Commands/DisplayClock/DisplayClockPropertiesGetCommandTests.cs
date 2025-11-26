using AtemSharp.Commands.DisplayClock;
using AtemSharp.State.DisplayClock;
using JetBrains.Annotations;

namespace AtemSharp.Tests.Commands.DisplayClock;

[TestFixture]
public class DisplayClockPropertiesGetCommandTests : DeserializedCommandTestBase<DisplayClockPropertiesGetCommand,
    DisplayClockPropertiesGetCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public bool Enabled { get; set; }
        public byte Size { get; set; }
        public byte Opacity { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public bool AutoHide { get; set; }
        public TimeData StartFrom { get; set; } = new();
        public DisplayClockClockMode ClockMode { get; set; }
        public DisplayClockClockState ClockState { get; set; }
    }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class TimeData
    {
        public byte Hour { get; set; }
        public byte Minute { get; set; }
        public byte Second { get; set; }
        public byte Frame { get; set; }
    }

    protected override void CompareCommandProperties(DisplayClockPropertiesGetCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.Enabled, Is.EqualTo(expectedData.Enabled));
        Assert.That(actualCommand.Size, Is.EqualTo(expectedData.Size));
        Assert.That(actualCommand.Opacity, Is.EqualTo(expectedData.Opacity));
        Assert.That(actualCommand.PositionX, Is.EqualTo(expectedData.PositionX).Within(0.01));
        Assert.That(actualCommand.PositionY, Is.EqualTo(expectedData.PositionY).Within(0.01));
        Assert.That(actualCommand.AutoHide, Is.EqualTo(expectedData.AutoHide));
        Assert.That(actualCommand.StartFromHours, Is.EqualTo(expectedData.StartFrom.Hour));
        Assert.That(actualCommand.StartFromMinutes, Is.EqualTo(expectedData.StartFrom.Minute));
        Assert.That(actualCommand.StartFromSeconds, Is.EqualTo(expectedData.StartFrom.Second));
        Assert.That(actualCommand.StartFromFrames, Is.EqualTo(expectedData.StartFrom.Frame));
        Assert.That(actualCommand.ClockMode, Is.EqualTo(expectedData.ClockMode));
        Assert.That(actualCommand.ClockState, Is.EqualTo(expectedData.ClockState));
    }
}
