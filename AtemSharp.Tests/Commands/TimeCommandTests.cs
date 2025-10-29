using AtemSharp.Commands;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands;

public class TimeCommandTests : SerializedCommandTestBase<TimeCommand, TimeCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Hour { get; set; }
        public byte Minute { get; set; }
        public byte Second { get; set; }
        public byte Frame { get; set; }
        public bool IsDropFrame { get; set; }
    }

    protected override TimeCommand CreateSut(TestCaseData testCase)
    {
        return new TimeCommand(new AtemState()
        {
            TimeCode =
            {
                Hours = testCase.Command.Hour,
                Minutes = testCase.Command.Minute,
                Seconds = testCase.Command.Second,
                Frames = testCase.Command.Frame,
                IsDropFrame = testCase.Command.IsDropFrame
            }
        });
    }
}
