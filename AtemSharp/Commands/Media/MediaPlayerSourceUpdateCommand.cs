using AtemSharp.State;
using AtemSharp.State.Media;

namespace AtemSharp.Commands.Media;

[Command("MPCE")]
internal partial class MediaPlayerSourceUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _mediaPlayerId;

    [DeserializedField(1)] private MediaSourceType _sourceType;

    [DeserializedField(2)] private byte _stillIndex;

    [DeserializedField(3)] private byte _clipIndex;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var player = state.Media.Players[MediaPlayerId];
        player.SourceType = SourceType;
        player.StillIndex = StillIndex;
        player.ClipIndex = ClipIndex;
    }
}
