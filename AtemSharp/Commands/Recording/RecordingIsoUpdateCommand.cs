using AtemSharp.State;
using AtemSharp.State.Info;

namespace AtemSharp.Commands.Recording;

[Command("ISOi", ProtocolVersion.V8_1_1)]
public partial class RecordingIsoUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)]
    private bool _recordAllInputs;

    public void ApplyToState(AtemState state)
    {
        state.Recording.RecordAllInputs = _recordAllInputs;
    }
}
