using AtemSharp.Commands.Media;
using AtemSharp.State.Media;

namespace AtemSharp.Tests.Commands.Media;

public class MediaPlayerStatusCommandTests : SerializedCommandTestBase<MediaPlayerStatusCommand, MediaPlayerStatusCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public bool Playing { get; set; }
        public bool Loop { get; set; }
        public bool AtBeginning { get; set; }
        public ushort ClipFrame { get; set; }
    }

    protected override MediaPlayerStatusCommand CreateSut(TestCaseData testCase)
    {
        return new MediaPlayerStatusCommand(new MediaPlayer
        {
            Id = testCase.Command.Index,
            IsPlaying = testCase.Command.Playing,
            IsLooping = testCase.Command.Loop,
            IsAtBeginning = testCase.Command.AtBeginning,
            ClipFrame = testCase.Command.ClipFrame
        });
    }
}
