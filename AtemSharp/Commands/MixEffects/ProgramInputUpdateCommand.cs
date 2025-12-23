using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects;

[Command("PrgI")]
internal partial class ProgramInputUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _mixEffectId;

    [DeserializedField(2)] private ushort _source;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.Video.MixEffects[MixEffectId].ProgramInput = Source;
    }
}
