using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Media;

[Command("MPSp", ProtocolVersion.V8_0)]
public partial class MediaPoolSettingsGetCommand : IDeserializedCommand
{
    [CustomDeserialization] private ushort[] _maxFrames = [];
    [DeserializedField(8)] private ushort _unassignedFrames;

    public void DeserializeInternal(ReadOnlySpan<byte> buffer, ProtocolVersion _)
    {
        _maxFrames =
        [
            buffer.ReadUInt16BigEndian(0),
            buffer.ReadUInt16BigEndian(2),
            buffer.ReadUInt16BigEndian(4),
            buffer.ReadUInt16BigEndian(6),
        ];
    }

    public void ApplyToState(AtemState state)
    {
        state.Settings.MediaPool.MaxFrames = _maxFrames;
        state.Settings.MediaPool.UnassignedFrames = _unassignedFrames;
    }
}
