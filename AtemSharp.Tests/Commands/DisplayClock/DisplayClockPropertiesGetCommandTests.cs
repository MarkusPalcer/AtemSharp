using AtemSharp.Commands.DisplayClock;
using AtemSharp.State;
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

    protected override void CompareCommandProperties(DisplayClockPropertiesGetCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
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

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        Assert.That(state.DisplayClock.Enabled, Is.EqualTo(expectedData.Enabled));
        Assert.That(state.DisplayClock.Size, Is.EqualTo(expectedData.Size));
        Assert.That(state.DisplayClock.Opacity, Is.EqualTo(expectedData.Opacity));
        Assert.That(state.DisplayClock.PositionX, Is.EqualTo(expectedData.PositionX).Within(0.01));
        Assert.That(state.DisplayClock.PositionY, Is.EqualTo(expectedData.PositionY).Within(0.01));
        Assert.That(state.DisplayClock.AutoHide, Is.EqualTo(expectedData.AutoHide));
        Assert.That(state.DisplayClock.StartFrom.Hours, Is.EqualTo(expectedData.StartFrom.Hour));
        Assert.That(state.DisplayClock.StartFrom.Minutes, Is.EqualTo(expectedData.StartFrom.Minute));
        Assert.That(state.DisplayClock.StartFrom.Seconds, Is.EqualTo(expectedData.StartFrom.Second));
        Assert.That(state.DisplayClock.StartFrom.Frames, Is.EqualTo(expectedData.StartFrom.Frame));
        Assert.That(state.DisplayClock.ClockMode, Is.EqualTo(expectedData.ClockMode));
        Assert.That(state.DisplayClock.ClockState, Is.EqualTo(expectedData.ClockState));
    }
}
