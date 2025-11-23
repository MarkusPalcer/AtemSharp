using AtemSharp.Commands.Media;

namespace AtemSharp.Tests.Commands.Media;

public class MediaPlayerStatusUpdateCommandTests : DeserializedCommandTestBase<MediaPlayerStatusUpdateCommand,
    MediaPlayerStatusUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public bool Playing { get; set; }
        public bool Loop { get; set; }
        public bool AtBeginning { get; set; }
        public ushort ClipFrame { get; set; }
    }

    protected override void CompareCommandProperties(MediaPlayerStatusUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.MediaPlayerId, Is.EqualTo(testCase.Command.Index));
        Assert.That(actualCommand.IsPlaying, Is.EqualTo(testCase.Command.Playing));
        Assert.That(actualCommand.IsLooping, Is.EqualTo(testCase.Command.Loop));
        Assert.That(actualCommand.IsAtBeginning, Is.EqualTo(testCase.Command.AtBeginning));
        Assert.That(actualCommand.ClipFrame, Is.EqualTo(testCase.Command.ClipFrame));
    }
}
