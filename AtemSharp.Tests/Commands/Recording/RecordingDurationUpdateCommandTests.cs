using AtemSharp.Commands.Recording;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Recording;

internal class RecordingDurationUpdateCommandTests : DeserializedCommandTestBase<RecordingDurationUpdateCommand, RecordingDurationUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Hour { get; set; }
        public byte Minute { get; set; }
        public byte Second { get; set; }
        public byte Frame { get; set; }
        public bool IsDropFrame { get; set; }
    }

    internal override void CompareCommandProperties(RecordingDurationUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.Hours, Is.EqualTo(expectedData.Hour));
        Assert.That(actualCommand.Minutes, Is.EqualTo(expectedData.Minute));
        Assert.That(actualCommand.Seconds, Is.EqualTo(expectedData.Second));
        Assert.That(actualCommand.Frames, Is.EqualTo(expectedData.Frame));
        Assert.That(actualCommand.IsDropFrame, Is.EqualTo(expectedData.IsDropFrame));
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        Assert.That(state.Recording.Duration.Hours, Is.EqualTo(expectedData.Hour));
        Assert.That(state.Recording.Duration.Minutes, Is.EqualTo(expectedData.Minute));
        Assert.That(state.Recording.Duration.Seconds, Is.EqualTo(expectedData.Second));
        Assert.That(state.Recording.Duration.Frames, Is.EqualTo(expectedData.Frame));
        Assert.That(state.Recording.Duration.IsDropFrame, Is.EqualTo(expectedData.IsDropFrame));
    }
}
