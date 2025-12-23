using AtemSharp.Commands;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands;

internal class TimeUpdateCommandTests : DeserializedCommandTestBase<TimeUpdateCommand, TimeUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Hour { get; set; }
        public byte Minute { get; set; }
        public byte Second { get; set; }
        public byte Frame { get; set; }
        public bool IsDropFrame { get; set; }
    }

    internal override void CompareCommandProperties(TimeUpdateCommand actualCommand, CommandData expectedData, TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        Assert.That(actualCommand.Hours, Is.EqualTo(expectedData.Hour));
        Assert.That(actualCommand.Minutes, Is.EqualTo(expectedData.Minute));
        Assert.That(actualCommand.Seconds, Is.EqualTo(expectedData.Second));
        Assert.That(actualCommand.Frames, Is.EqualTo(expectedData.Frame));
        Assert.That(actualCommand.IsDropFrame, Is.EqualTo(expectedData.IsDropFrame));
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var actualCommand = state.TimeCode;
        Assert.That(actualCommand.Hours, Is.EqualTo(expectedData.Hour));
        Assert.That(actualCommand.Minutes, Is.EqualTo(expectedData.Minute));
        Assert.That(actualCommand.Seconds, Is.EqualTo(expectedData.Second));
        Assert.That(actualCommand.Frames, Is.EqualTo(expectedData.Frame));
        Assert.That(actualCommand.IsDropFrame, Is.EqualTo(expectedData.IsDropFrame));
    }
}
