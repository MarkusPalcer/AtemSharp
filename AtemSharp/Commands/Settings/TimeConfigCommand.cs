using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.Settings;

/// <summary>
/// Command to set the time configuration mode for the ATEM device
/// </summary>
[Command("CTCC", ProtocolVersion.V8_1_1)]
[BufferSize(4)]
public partial class TimeConfigCommand : SerializedCommand
{
    /// <summary>
    /// Time mode for the ATEM device
    /// </summary>
    [SerializedField(0)]
    private TimeMode _mode;

    /// <summary>
    /// Create command initialized with current state values
    /// </summary>
    /// <param name="currentState">Current ATEM state</param>
    public TimeConfigCommand(AtemState currentState)
    {
        _mode = currentState.Settings.TimeMode;
    }
}
