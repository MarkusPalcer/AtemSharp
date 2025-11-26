using AtemSharp.State;
using AtemSharp.State.Info;
using AtemSharp.State.Settings;

namespace AtemSharp.Commands.Settings;

/// <summary>
/// Update command for time configuration mode from the ATEM device
/// </summary>
[Command("TCCc", ProtocolVersion.V8_1_1)]
public partial class TimeConfigUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// Time mode for the ATEM device
    /// </summary>
    [DeserializedField(0)] private TimeMode _mode;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Update the state object
        state.Settings.TimeMode = Mode;
    }
}
