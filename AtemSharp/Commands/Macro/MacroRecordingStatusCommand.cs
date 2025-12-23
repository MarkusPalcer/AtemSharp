using AtemSharp.State;

namespace AtemSharp.Commands.Macro;

[Command("MRcS")]
internal partial class MacroRecordingStatusCommand : IDeserializedCommand
{
    [DeserializedField(0)] private bool _isRecording;

    [DeserializedField(2)] private ushort _macroIndex;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.Macros.Recorder.IsRecording = IsRecording;
        state.Macros.Recorder.MacroIndex = MacroIndex;
    }
}
