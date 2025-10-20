using AtemSharp.Commands.Media;

namespace AtemSharp.Tests.Commands.Media;

public class MediaPoolFrameDescriptionCommandTests : DeserializedCommandTestBase<MediaPoolFrameDescriptionCommand, MediaPoolFrameDescriptionCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int Bank { get; set; }
        public int Index { get; set; }
        public bool IsUsed { get; set; }
        public string Hash { get; set; } = string.Empty;
        public string Filename { get; set; } = string.Empty;
    }

    protected override void CompareCommandProperties(MediaPoolFrameDescriptionCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.MediaPoolId, Is.EqualTo((byte)expectedData.Bank));
        Assert.That(actualCommand.FrameIndex, Is.EqualTo((ushort)expectedData.Index));
        Assert.That(actualCommand.IsUsed, Is.EqualTo(expectedData.IsUsed));
        Assert.That(actualCommand.Hash, Is.EqualTo(expectedData.Hash));
        Assert.That(actualCommand.FileName, Is.EqualTo(expectedData.Filename));
    }
}
