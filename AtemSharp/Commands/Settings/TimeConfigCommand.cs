using AtemSharp.State;
using AtemSharp.State.Info;
using AtemSharp.State.Settings;

namespace AtemSharp.Commands.Settings;

/// <summary>
/// Used to set the time configuration mode for the ATEM device
/// </summary>
[Command("CTCC", ProtocolVersion.V8_1_1)]
[BufferSize(4)]
public partial class TimeConfigCommand(AtemState currentState) : SerializedCommand
{
    /// <summary>
    /// Time mode for the ATEM device
    /// </summary>
    [SerializedField(0)] private TimeMode _mode = currentState.Settings.TimeMode;
}
