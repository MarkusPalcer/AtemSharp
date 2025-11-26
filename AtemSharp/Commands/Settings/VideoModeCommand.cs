using AtemSharp.State;
using AtemSharp.State.Settings;

namespace AtemSharp.Commands.Settings;

/// <summary>
/// Command to set the video mode of the ATEM device
/// </summary>
[Command("CVdM")]
[BufferSize(4)]
public partial class VideoModeCommand(AtemState currentState) : SerializedCommand
{
    /// <summary>
    /// Video mode to set
    /// </summary>
    [SerializedField(0)] private VideoMode _mode = currentState.Settings.VideoMode;
}
