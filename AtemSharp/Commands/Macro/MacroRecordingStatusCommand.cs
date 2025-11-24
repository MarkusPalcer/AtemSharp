using AtemSharp.State;

namespace AtemSharp.Commands.Macro;

[Command("MRcS")]
public partial class MacroRecordingStatusCommand : IDeserializedCommand
{
    [DeserializedField(0)]
    private bool _isRecording;

    [DeserializedField(2)]
    private ushort _macroIndex;

    public void ApplyToState(AtemState state)
    {
        state.Macros.Recorder.IsRecording = IsRecording;
        state.Macros.Recorder.MacroIndex = MacroIndex;
    }
}
