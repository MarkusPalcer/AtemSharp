using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Media;

[Command("MPCE")]
public class MediaPlayerSourceUpdateCommand : IDeserializedCommand
{
    public MediaSourceType SourceType { get; private set; }
    public byte StillIndex { get; set; }
    public byte ClipIndex { get; set; }
    public byte MediaPlayerId { get; set; }

    public static IDeserializedCommand Deserialize(ReadOnlySpan<byte> data, ProtocolVersion version)
    {
        return new MediaPlayerSourceUpdateCommand()
        {
            MediaPlayerId = data.ReadUInt8(0),
            SourceType = (MediaSourceType)data.ReadUInt8(1),
            StillIndex = data.ReadUInt8(2),
            ClipIndex = data.ReadUInt8(3),
        };
    }

    public void ApplyToState(AtemState state)
    {
        var player = state.Media.Players[MediaPlayerId];
        player.SourceType = SourceType;
        player.StillIndex = StillIndex;
        player.ClipIndex = ClipIndex;
    }
}
