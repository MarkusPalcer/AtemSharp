using AtemSharp.State;
using AtemSharp.State.Info;
using AtemSharp.State.Settings;

namespace AtemSharp.Commands.Settings;

[Command("TCCc", ProtocolVersion.V8_1_1)]
internal partial class TimeConfigUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private TimeMode _mode;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Update the state object
        state.Settings.TimeMode = Mode;
    }
}
