using AtemSharp.Lib;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Fairlight;

public abstract class FairlightMixerSourceCommandBase : SerializedCommand
{
    protected ushort InputId { get; private set; }
    protected long SourceId { get; private set; }

    public FairlightMixerSourceCommandBase(Source source)
    {
        InputId = source.InputId;
        SourceId = source.Id;
    }

    protected void SerializeIds(byte[] buffer)
    {
        buffer.WriteUInt16BigEndian(InputId, 2);
        buffer.WriteInt64BigEndian(SourceId, 8);
    }
}
