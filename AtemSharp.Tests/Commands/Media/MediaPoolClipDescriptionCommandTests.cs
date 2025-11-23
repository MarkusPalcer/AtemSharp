using AtemSharp.Commands.Media;

namespace AtemSharp.Tests.Commands.Media;

public class MediaPoolClipDescriptionCommandTests : DeserializedCommandTestBase<MediaPoolClipDescriptionCommand , MediaPoolClipDescriptionCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public bool IsUsed { get; set; }
        public string Name { get; set; } = string.Empty;
        public ushort FrameCount { get; set; }
    }

    protected override void CompareCommandProperties(MediaPoolClipDescriptionCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.ClipId, Is.EqualTo(testCase.Command.Index));
        Assert.That(actualCommand.IsUsed, Is.EqualTo(testCase.Command.IsUsed));
        Assert.That(actualCommand.Name, Is.EqualTo(testCase.Command.Name));
        Assert.That(actualCommand.FrameCount, Is.EqualTo(testCase.Command.FrameCount));
    }
}
