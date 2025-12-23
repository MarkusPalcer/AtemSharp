using AtemSharp.State;
using AtemSharp.State.Settings;

namespace AtemSharp.Commands.Settings;

/// <summary>
/// Used to set the video mode of the ATEM device
/// </summary>
[Command("CVdM")]
[BufferSize(4)]
public partial class VideoModeCommand(AtemState currentState) : SerializedCommand
{
    [SerializedField(0)] private VideoMode _mode = currentState.Settings.VideoMode;
}
