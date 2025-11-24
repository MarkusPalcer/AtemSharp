using AtemSharp.State;

namespace AtemSharp.Commands.Media;

[Command("RCPS")]
public partial class MediaPlayerStatusUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _mediaPlayerId;
    [DeserializedField(1)] private bool _isPlaying;
    [DeserializedField(2)] private bool _isLooping;
    [DeserializedField(3)] private bool _isAtBeginning;
    [DeserializedField(4)] private ushort _clipFrame;

    public void ApplyToState(AtemState state)
    {
        var player = state.Media.Players[_mediaPlayerId];
        player.IsPlaying = _isPlaying;
        player.IsLooping = _isLooping;
        player.IsAtBeginning = _isAtBeginning;
        player.ClipFrame = _clipFrame;
    }
}
