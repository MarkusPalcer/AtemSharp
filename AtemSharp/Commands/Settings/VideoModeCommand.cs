using AtemSharp.Enums;
using AtemSharp.Helpers;
using AtemSharp.State;

namespace AtemSharp.Commands.Settings;

/// <summary>
/// Command to set the video mode of the ATEM device
/// </summary>
[Command("CVdM")]
[BufferSize(4)]
public partial class VideoModeCommand : SerializedCommand
{
    /// <summary>
    /// Video mode to set
    /// </summary>
    [SerializedField(0)]
    private VideoMode _mode;

    /// <summary>
    /// Create command initialized with current state values
    /// </summary>
    /// <param name="currentState">Current ATEM state</param>
    public VideoModeCommand(AtemState currentState)
    {
        // Initialize from current state (direct field access = no flags set)
        _mode = currentState.Settings.VideoMode;
    }
}
