using AtemSharp.Commands.Media;
using AtemSharp.State;
using AtemSharp.State.Media;

namespace AtemSharp.Tests.Commands.Media;

public class MediaPlayerSourceUpdateCommandTests : DeserializedCommandTestBase<MediaPlayerSourceUpdateCommand,
    MediaPlayerSourceUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public MediaSourceType SourceType { get; set; }
        public uint SourceIndex { get; set; }
    }

    protected override void CompareCommandProperties(MediaPlayerSourceUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.SourceType, Is.EqualTo(expectedData.SourceType));
        Assert.That(actualCommand.MediaPlayerId, Is.EqualTo(expectedData.Index));

        if (expectedData.SourceType == MediaSourceType.Still)
        {
            Assert.That(actualCommand.StillIndex, Is.EqualTo(expectedData.SourceIndex));
        }
        else
        {
            Assert.That(actualCommand.ClipIndex, Is.EqualTo(expectedData.SourceIndex));
        }
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Media.Players = AtemStateUtil.CreateArray<MediaPlayer>(expectedData.Index + 1);
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var mediaPlayer = state.Media.Players[expectedData.Index];

        Assert.That(mediaPlayer.Id, Is.EqualTo(expectedData.Index));
        Assert.That(mediaPlayer.SourceType, Is.EqualTo(expectedData.SourceType));

        if (expectedData.SourceType == MediaSourceType.Still)
        {
            Assert.That(mediaPlayer.StillIndex, Is.EqualTo(expectedData.SourceIndex));
        }
        else
        {
            Assert.That(mediaPlayer.ClipIndex, Is.EqualTo(expectedData.SourceIndex));
        }
    }
}
