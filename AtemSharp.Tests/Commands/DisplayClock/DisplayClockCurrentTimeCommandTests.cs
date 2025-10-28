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
        var failures = new List<string>();

        // Compare Time.Hour
        if (actualCommand.Hours != expectedData.Time.Hour)
        {
            failures.Add($"Time.Hour: expected {expectedData.Time.Hour}, actual {actualCommand.Hours}");
        }

        // Compare Time.Minute
        if (actualCommand.Minutes != expectedData.Time.Minute)
        {
            failures.Add($"Time.Minute: expected {expectedData.Time.Minute}, actual {actualCommand.Minutes}");
        }

        // Compare Time.Second
        if (actualCommand.Seconds != expectedData.Time.Second)
        {
            failures.Add($"Time.Second: expected {expectedData.Time.Second}, actual {actualCommand.Seconds}");
        }

        // Compare Time.Frame
        if (actualCommand.Frames != expectedData.Time.Frame)
        {
            failures.Add($"Time.Frame: expected {expectedData.Time.Frame}, actual {actualCommand.Frames}");
        }

        if (failures.Any())
        {
            Assert.Fail($"Command property comparison failed:\n{string.Join("\n", failures)}");
        }
    }
}
