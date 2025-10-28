using AtemSharp.Enums;
using AtemSharp.Helpers;
using AtemSharp.State;

namespace AtemSharp.Commands.DownstreamKey;

[Command("DskS", ProtocolVersion.V8_0_1)]
public partial class DownstreamKeyStateV8Command : IDeserializedCommand
{
    [DeserializedField(0)]
    private byte _index;

    [DeserializedField(1)]
    private bool _onAir;

    [DeserializedField(2)]
    private bool _inTransition;

    [DeserializedField(3)]
    private bool _isAuto;

    [DeserializedField(4)]
    private bool _isTowardsOnAir;

    [DeserializedField(5)]
    private byte _remainingFrames;

    public virtual void ApplyToState(AtemState state)
    {
        var dsk = state.Video.DownstreamKeyers[Index];
        dsk.OnAir = OnAir;
        dsk.InTransition = InTransition;
        dsk.IsAuto = IsAuto;
        dsk.RemainingFrames = RemainingFrames;
        dsk.IsTowardsOnAir = IsTowardsOnAir;
    }
}
