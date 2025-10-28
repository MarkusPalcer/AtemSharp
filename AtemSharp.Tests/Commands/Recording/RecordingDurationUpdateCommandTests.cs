using AtemSharp.Commands.Recording;

namespace AtemSharp.Tests.Commands.Recording;

public class RecordingDurationUpdateCommandTests : DeserializedCommandTestBase<RecordingDurationUpdateCommand, RecordingDurationUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Hour { get; set; }
        public byte Minute { get; set; }
        public byte Second { get; set; }
        public byte Frame { get; set; }
        public bool IsDropFrame { get; set; }
    }

    protected override void CompareCommandProperties(RecordingDurationUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.Hours, Is.EqualTo(expectedData.Hour));
        Assert.That(actualCommand.Minutes, Is.EqualTo(expectedData.Minute));
        Assert.That(actualCommand.Seconds, Is.EqualTo(expectedData.Second));
        Assert.That(actualCommand.Frames, Is.EqualTo(expectedData.Frame));
        Assert.That(actualCommand.IsDropFrame, Is.EqualTo(expectedData.IsDropFrame));
    }
}
