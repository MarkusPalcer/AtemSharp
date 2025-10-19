using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Macro;

[Command("MRcS")]
public class MacroRecordingStatusCommand : IDeserializedCommand
{
    public static IDeserializedCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        return new MacroRecordingStatusCommand
        {
            IsRecording = rawCommand.ReadBoolean(0),
            MacroIndex = rawCommand.ReadUInt16BigEndian(2),
        };
    }

    public ushort MacroIndex { get; set; }

    public bool IsRecording { get; set; }

    public void ApplyToState(AtemState state)
    {
        state.Macros.Recorder.IsRecording = IsRecording;
        state.Macros.Recorder.MacroIndex = MacroIndex;
    }
}
