using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Media;

[Command("MPSS")]
public class MediaPlayerSourceCommand : SerializedCommand
{
    private MediaSourceType _sourceType;
    private byte _stillIndex;
    private byte _clipIndex;

    public MediaSourceType SourceType
    {
        get => _sourceType;
        set
        {
            _sourceType = value;
            Flag |= 1 << 0;
        }
    }

    public byte StillIndex
    {
        get => _stillIndex;
        set
        {
            _stillIndex = value;
            Flag |= 1 << 1;
        }
    }

    public byte ClipIndex
    {
        get => _clipIndex;
        set
        {
            _clipIndex = value;
            Flag |= 1 << 2;
        }
    }

    public byte MediaPlayerId { get; private set; }

    public MediaPlayerSourceCommand(MediaPlayer player)
    {
        _sourceType = player.SourceType;
        _stillIndex = player.StillIndex;
        _clipIndex = player.ClipIndex;

        MediaPlayerId = player.Id;
    }

    public MediaPlayerSourceCommand(AtemState state, int mediaPlayerId)
        : this(state.Media.Players[mediaPlayerId])
    {
    }

    public override byte[] Serialize(ProtocolVersion version)
    {
        var buffer = new byte[8];
        buffer.WriteUInt8((byte)Flag, 0);
        buffer.WriteUInt8(MediaPlayerId, 1);
        buffer.WriteUInt8((byte)SourceType, 2);
        buffer.WriteUInt8(StillIndex, 3);
        buffer.WriteUInt8(ClipIndex, 4);

        return buffer;
    }
}
