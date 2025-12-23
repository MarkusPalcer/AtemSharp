using AtemSharp.State;

namespace AtemSharp.Commands.Video;

[Command("AuxS")]
internal partial class AuxSourceUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _auxId;
    [DeserializedField(2)] private ushort _source;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.Video.Auxiliaries[AuxId].Source = Source;
    }
}
