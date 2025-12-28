using System.Diagnostics.CodeAnalysis;
using AtemSharp.State;

namespace AtemSharp.Commands.Macro;

[Command("MRcS")]
internal partial class MacroRecordingStatusCommand : IDeserializedCommand
{
    [DeserializedField(0)] private bool _isRecording;

    [DeserializedField(2)] private ushort _macroIndex;

    [ExcludeFromCodeCoverage(Justification = "Obsolete")]
    public void ApplyToState(AtemState state)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public void Apply(IStateHolder state)
    {
        state.Macros.Recorder.UpdateCurrentlyRecording(IsRecording ? state.Macros[_macroIndex] : null);
    }
}
