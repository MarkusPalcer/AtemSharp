using AtemSharp.State;
using AtemSharp.State.Info;

namespace AtemSharp.Commands.Media;


[Command("MPSp", ProtocolVersion.V8_0)]
internal partial class MediaPoolSettingsGetCommand : IDeserializedCommand
{
    [CustomDeserialization] private ushort[] _maxFrames = [];
    [DeserializedField(8)] private ushort _unassignedFrames;

    private void DeserializeInternal(ReadOnlySpan<byte> buffer)
    {
        _maxFrames =
        [
            buffer.ReadUInt16BigEndian(0),
            buffer.ReadUInt16BigEndian(2),
            buffer.ReadUInt16BigEndian(4),
            buffer.ReadUInt16BigEndian(6),
        ];
    }

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.Settings.MediaPool.MaxFrames = _maxFrames;
        state.Settings.MediaPool.UnassignedFrames = _unassignedFrames;
    }
}
