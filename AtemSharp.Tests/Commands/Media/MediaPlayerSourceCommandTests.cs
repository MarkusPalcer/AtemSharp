using AtemSharp.Commands.Media;
using AtemSharp.State.Media;

namespace AtemSharp.Tests.Commands.Media;

public class MediaPlayerSourceCommandTests : SerializedCommandTestBase<MediaPlayerSourceCommand, MediaPlayerSourceCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public MediaSourceType SourceType { get; set; }
        public byte StillIndex { get; set; }
        public byte ClipIndex { get; set; }
    }

    protected override MediaPlayerSourceCommand CreateSut(TestCaseData testCase)
    {
        return new MediaPlayerSourceCommand(new MediaPlayer
        {
            Id = testCase.Command.Index,
            SourceType = testCase.Command.SourceType,
            StillIndex = testCase.Command.StillIndex,
            ClipIndex = testCase.Command.ClipIndex
        });
    }
}
