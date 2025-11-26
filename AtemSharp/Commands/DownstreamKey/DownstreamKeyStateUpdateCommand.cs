using AtemSharp.State;
using AtemSharp.State.Info;

namespace AtemSharp.Commands.DownstreamKey;

[Command("DskS")]
public class DownstreamKeyStateUpdateCommand : IDeserializedCommand
{
    public byte Index { get; init; }
    public bool InTransition { get; init; }
    public bool OnAir { get; init; }
    public bool IsAuto { get; init; }
    public byte RemainingFrames { get; init; }

    public static IDeserializedCommand Deserialize(ReadOnlySpan<byte> data, ProtocolVersion version)
    {
        return new DownstreamKeyStateUpdateCommand
        {
            Index = data.ReadUInt8(0),
            OnAir = data.ReadBoolean(1),
            InTransition = data.ReadBoolean(2),
            IsAuto = data.ReadBoolean(3),
            RemainingFrames = data.ReadUInt8(4)
        };
    }

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var dsk = state.Video.DownstreamKeyers[Index];
        dsk.OnAir = OnAir;
        dsk.InTransition = InTransition;
        dsk.IsAuto = IsAuto;
        dsk.RemainingFrames = RemainingFrames;
    }
}
