using AtemSharp.State;

namespace AtemSharp.Commands.Settings.MultiViewers;

[Command("VuMo")]
[BufferSize(4)]
internal partial class MultiViewerVuOpacityUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _multiViewerId;
    [DeserializedField(1)] private byte _opacity;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.Settings.MultiViewers[_multiViewerId].VuOpacity = Opacity;
    }
}
