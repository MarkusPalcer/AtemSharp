using AtemSharp.Commands.DisplayClock;

namespace AtemSharp.Tests.Commands.DisplayClock;

[TestFixture]
public class DisplayClockCurrentTimeCommandTests : DeserializedCommandTestBase<DisplayClockCurrentTimeCommand,
    DisplayClockCurrentTimeCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public TimeData Time { get; set; } = new();
    }

    public class TimeData
    {
        public byte Hour { get; set; }
        public byte Minute { get; set; }
        public byte Second { get; set; }
        public byte Frame { get; set; }
    }

    protected override void CompareCommandProperties(DisplayClockCurrentTimeCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.Hours, Is.EqualTo(expectedData.Time.Hour));
        Assert.That(actualCommand.Minutes, Is.EqualTo(expectedData.Time.Minute));
        Assert.That(actualCommand.Seconds, Is.EqualTo(expectedData.Time.Second));
        Assert.That(actualCommand.Frames, Is.EqualTo(expectedData.Time.Frame));
    }
}
