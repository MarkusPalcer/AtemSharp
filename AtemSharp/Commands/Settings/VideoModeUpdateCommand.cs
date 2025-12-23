using AtemSharp.State;
using AtemSharp.State.Settings;

namespace AtemSharp.Commands.Settings;

[Command("VidM")]
internal partial class VideoModeUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private VideoMode _mode;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.Settings.VideoMode = Mode;
    }
}
