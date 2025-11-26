using AtemSharp.State;
using AtemSharp.State.Settings;

namespace AtemSharp.Commands.Settings;

/// <summary>
/// Command received from ATEM device containing video mode update
/// </summary>
[Command("VidM")]
public partial class VideoModeUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// Video mode of the device
    /// </summary>
    [DeserializedField(0)] private VideoMode _mode;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.Settings.VideoMode = Mode;
    }
}
