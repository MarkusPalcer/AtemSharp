using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.DownstreamKey;

[Command("DskS", ProtocolVersion.V8_0_1)]
public class DownstreamKeyStateV8Command : IDeserializedCommand
{
    public byte Index { get; init; }
    public bool InTransition { get; init; }
    public bool OnAir { get; init; }
    public bool IsAuto { get; init; }
    public byte RemainingFrames { get; init; }
    public bool IsTowardsOnAir { get; init; }

    public static IDeserializedCommand Deserialize(ReadOnlySpan<byte> data, ProtocolVersion version)
    {
        return new DownstreamKeyStateV8Command
        {
            Index = data.ReadUInt8(0),
            OnAir = data.ReadBoolean(1),
            InTransition = data.ReadBoolean(2),
            IsAuto = data.ReadBoolean(3),
            IsTowardsOnAir = data.ReadBoolean(4),
            RemainingFrames = data.ReadUInt8(5)
        };
    }

    public virtual void ApplyToState(AtemState state)
    {
        if (state.Info.Capabilities?.DownstreamKeyers is null || Index >= state.Info.Capabilities.DownstreamKeyers)
        {
            throw new InvalidOperationException("Either the number of downstream keyers is unknown or the index is out of range");
        }

        var dsk = state.Video.DownstreamKeyers[Index];
        dsk.OnAir = OnAir;
        dsk.InTransition = InTransition;
        dsk.IsAuto = IsAuto;
        dsk.RemainingFrames = RemainingFrames;
        dsk.IsTowardsOnAir = IsTowardsOnAir;
    }
}
