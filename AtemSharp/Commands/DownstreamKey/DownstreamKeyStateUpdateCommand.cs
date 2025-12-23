using AtemSharp.State;

namespace AtemSharp.Commands.DownstreamKey;

[Command("DskS")]
internal partial class DownstreamKeyStateUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _index;
    [DeserializedField(1)] private bool _onAir;
    [DeserializedField(2)] private bool _inTransition;
    [DeserializedField(3)] private bool _isAuto;
    [DeserializedField(4)] private byte _remainingFrames;

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
