using AtemSharp.Commands.DisplayClock;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.DisplayClock;

[TestFixture]
internal class DisplayClockCurrentTimeCommandTests : DeserializedCommandTestBase<DisplayClockCurrentTimeCommand,
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

    internal override void CompareCommandProperties(DisplayClockCurrentTimeCommand actualCommand, CommandData expectedData)
    {
        Assert.That(actualCommand.Hours, Is.EqualTo(expectedData.Time.Hour));
        Assert.That(actualCommand.Minutes, Is.EqualTo(expectedData.Time.Minute));
        Assert.That(actualCommand.Seconds, Is.EqualTo(expectedData.Time.Second));
        Assert.That(actualCommand.Frames, Is.EqualTo(expectedData.Time.Frame));
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var actualCommand = state.DisplayClock.CurrentTime;
        Assert.That(actualCommand.Hours, Is.EqualTo(expectedData.Time.Hour));
        Assert.That(actualCommand.Minutes, Is.EqualTo(expectedData.Time.Minute));
        Assert.That(actualCommand.Seconds, Is.EqualTo(expectedData.Time.Second));
        Assert.That(actualCommand.Frames, Is.EqualTo(expectedData.Time.Frame));
    }
}
