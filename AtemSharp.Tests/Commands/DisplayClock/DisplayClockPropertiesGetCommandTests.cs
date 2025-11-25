using AtemSharp.Commands.DisplayClock;
using AtemSharp.State.DisplayClock;
using AtemSharp.Tests.TestUtilities;
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
        var failures = new List<string>();

        // Compare Enabled
        if (actualCommand.Enabled != expectedData.Enabled)
        {
            failures.Add($"Enabled: expected {expectedData.Enabled}, actual {actualCommand.Enabled}");
        }

        // Compare Size
        if (actualCommand.Size != expectedData.Size)
        {
            failures.Add($"Size: expected {expectedData.Size}, actual {actualCommand.Size}");
        }

        // Compare Opacity
        if (actualCommand.Opacity != expectedData.Opacity)
        {
            failures.Add($"Opacity: expected {expectedData.Opacity}, actual {actualCommand.Opacity}");
        }

        // Compare PositionX (floating point)
        if (!Utilities.AreApproximatelyEqual(actualCommand.PositionX, expectedData.PositionX))
        {
            failures.Add($"PositionX: expected {expectedData.PositionX}, actual {actualCommand.PositionX}");
        }

        // Compare PositionY (floating point)
        if (!Utilities.AreApproximatelyEqual(actualCommand.PositionY, expectedData.PositionY))
        {
            failures.Add($"PositionY: expected {expectedData.PositionY}, actual {actualCommand.PositionY}");
        }

        // Compare AutoHide
        if (actualCommand.AutoHide != expectedData.AutoHide)
        {
            failures.Add($"AutoHide: expected {expectedData.AutoHide}, actual {actualCommand.AutoHide}");
        }

        // Compare StartFrom.Hours
        if (actualCommand.StartFromHours != expectedData.StartFrom.Hour)
        {
            failures.Add($"StartFrom.Hours: expected {expectedData.StartFrom.Hour}, actual {actualCommand.StartFromHours}");
        }

        // Compare StartFrom.Minutes
        if (actualCommand.StartFromMinutes != expectedData.StartFrom.Minute)
        {
            failures.Add($"StartFrom.Minutes: expected {expectedData.StartFrom.Minute}, actual {actualCommand.StartFromMinutes}");
        }

        // Compare StartFrom.Seconds
        if (actualCommand.StartFromSeconds != expectedData.StartFrom.Second)
        {
            failures.Add($"StartFrom.Seconds: expected {expectedData.StartFrom.Second}, actual {actualCommand.StartFromSeconds}");
        }

        // Compare StartFrom.Frames
        if (actualCommand.StartFromFrames != expectedData.StartFrom.Frame)
        {
            failures.Add($"StartFrom.Frames: expected {expectedData.StartFrom.Frame}, actual {actualCommand.StartFromFrames}");
        }

        // Compare ClockMode
        if (actualCommand.ClockMode != expectedData.ClockMode)
        {
            failures.Add($"ClockMode: expected {expectedData.ClockMode}, actual {actualCommand.ClockMode}");
        }

        // Compare ClockState
        if (actualCommand.ClockState != expectedData.ClockState)
        {
            failures.Add($"ClockState: expected {expectedData.ClockState}, actual {actualCommand.ClockState}");
        }

        if (failures.Any())
        {
            Assert.Fail($"Command property comparison failed:\n{string.Join("\n", failures)}");
        }
    }
}
