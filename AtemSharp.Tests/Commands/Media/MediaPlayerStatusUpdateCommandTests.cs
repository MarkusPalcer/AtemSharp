using AtemSharp.Commands.Media;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Media;

internal class MediaPlayerStatusUpdateCommandTests : DeserializedCommandTestBase<MediaPlayerStatusUpdateCommand,
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

    internal override void CompareCommandProperties(MediaPlayerStatusUpdateCommand actualCommand, CommandData expectedData)
    {
        Assert.That(actualCommand.MediaPlayerId, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.IsPlaying, Is.EqualTo(expectedData.Playing));
        Assert.That(actualCommand.IsLooping, Is.EqualTo(expectedData.Loop));
        Assert.That(actualCommand.IsAtBeginning, Is.EqualTo(expectedData.AtBeginning));
        Assert.That(actualCommand.ClipFrame, Is.EqualTo(expectedData.ClipFrame));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Media.Players.GetOrCreate(expectedData.Index);
    }


    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var actualCommand = state.Media.Players[expectedData.Index];
        Assert.That(actualCommand.Id, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.IsPlaying, Is.EqualTo(expectedData.Playing));
        Assert.That(actualCommand.IsLooping, Is.EqualTo(expectedData.Loop));
        Assert.That(actualCommand.IsAtBeginning, Is.EqualTo(expectedData.AtBeginning));
        Assert.That(actualCommand.ClipFrame, Is.EqualTo(expectedData.ClipFrame));
    }
}
