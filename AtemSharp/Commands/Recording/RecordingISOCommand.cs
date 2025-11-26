using AtemSharp.State;
using AtemSharp.State.Info;

namespace AtemSharp.Commands.Recording;

[Command("ISOi", ProtocolVersion.V8_1_1)]
[BufferSize(4)]
public partial class RecordingIsoCommand(AtemState state) : SerializedCommand
{
    [SerializedField(0)] private bool _recordAllInputs = state.Recording.RecordAllInputs;
}
